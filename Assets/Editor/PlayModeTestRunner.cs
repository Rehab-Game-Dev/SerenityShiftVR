using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Unity.AI.Assistant.PlayModeTest
{
    [InitializeOnLoad]
    internal static class PlayModeTestRunner
    {
        private const string StateKey = "PlayModeTest.State";
        private const string ResultKey = "PlayModeTest.Result";
        private static readonly int WaitFrames = SessionState.GetInt("PlayModeTest.WaitFrames", 20);
        private static List<string> _capturedLogs = new List<string>();

        static PlayModeTestRunner()
        {
            string state = SessionState.GetString(StateKey, "Idle");
            if (state == "WaitingForCompile")
            {
                EditorApplication.delayCall += () => {
                    SessionState.SetString(StateKey, "EnteringPlayMode");
                    EditorApplication.isPlaying = true;
                };
            }
            else if (state == "EnteringPlayMode" && EditorApplication.isPlaying)
            {
                SessionState.SetString(StateKey, "InPlayMode");
                EditorApplication.update += WaitFramesThenRun;
            }
        }

        private static int _frameCount = 0;
        private static bool _testDone = false;
        private static double _startTime = 0;
        private static GameObject _overlay;

        private static void WaitFramesThenRun()
        {
            if (_testDone) return;
            _frameCount++;
            if (_frameCount < WaitFrames) return;

            if (_startTime == 0)
            {
                _startTime = EditorApplication.timeSinceStartup;
                Application.logMessageReceived += (msg, stack, type) => _capturedLogs.Add("[" + type + "] " + msg);
                
                var pm = Object.FindFirstObjectByType<PauseManager>();
                if (pm != null) _overlay = pm.pauseOverlay;
                
                Debug.Log("[Test] Active Manager: " + (pm != null ? pm.gameObject.name : "NONE"));
                Debug.Log("[Test] Simulating M key press");
                
                var kb = InputSystem.GetDevice<Keyboard>();
                if (kb != null)
                {
                    using (StateEvent.From(kb, out var eventPtr))
                    {
                        kb.mKey.WriteValueIntoEvent(1f, eventPtr);
                        InputSystem.QueueEvent(eventPtr);
                    }
                }
            }

            float elapsed = (float)(EditorApplication.timeSinceStartup - _startTime);
            if (elapsed > 2.0f)
            {
                _testDone = true;
                Finish();
            }
        }

        private static void Finish()
        {
            var r = new TestResult();
            r.overlayActive = _overlay != null && _overlay.activeInHierarchy;
            r.logs = _capturedLogs.ToArray();
            r.success = r.overlayActive;

            SessionState.SetString(ResultKey, JsonUtility.ToJson(r));
            SessionState.SetString(StateKey, "Done");
            EditorApplication.isPlaying = false;
        }

        [System.Serializable]
        private class TestResult { public bool success; public bool overlayActive; public string[] logs; }
    }
}
