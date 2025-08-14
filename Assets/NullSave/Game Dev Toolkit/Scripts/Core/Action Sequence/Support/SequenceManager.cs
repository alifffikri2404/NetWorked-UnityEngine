using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    public class SequenceManager
    {

        #region Fields

        public SequenceComplete onSequenceComplete;
        public GameObject remoteTarget;

        private List<ActionSequenceList> sequences;

        private ActionSequence sequenceRunner;
        private int index;
        private bool m_isPlaying;

        #endregion

        #region Properties

        public bool isPaused { get; private set; }

        public bool isPlaying
        {
            get
            {
                if(m_isPlaying && index >= sequences.Count)
                {
                    m_isPlaying = false;
                    if(sequenceRunner.isStarted)
                    {
                        sequenceRunner.Stop();
                    }
                }
                return m_isPlaying;
            }
            private set
            {
                m_isPlaying = value;
            }
        }

        public Transform parentTo { get; set; }

        public int sequenceCount { get { return sequences.Count; } }

        #endregion

        #region Constructor

        public SequenceManager()
        {
            sequences = new List<ActionSequenceList>();
        }

        public SequenceManager(List<ActionSequenceList> sequenceList)
        {
            sequences = new List<ActionSequenceList>();
            sequences.AddRange(sequenceList);
        }

        public SequenceManager(ActionSequenceList[] sequenceList)
        {
            sequences = new List<ActionSequenceList>();
            sequences.AddRange(sequenceList);
        }

        #endregion

        #region Public Methods

        public void AddSequence(ActionSequenceList sequence)
        {
            sequences.Add(sequence);
        }

        public void Clear()
        {
            if (isPlaying)
            {
                return;
            }

            sequences.Clear();
            index = 0;
        }

        public void Pause()
        {
            if (!isPlaying) return;
            isPaused = true;
        }

        public void Play()
        {
            if (isPlaying) return;

            if (sequenceRunner == null)
            {
                GameObject go = new GameObject("GDTK_SequenceManager");
                go.transform.SetParent(parentTo);
                go.transform.localPosition = Vector3.zero;
                go.hideFlags = HideFlags.HideAndDontSave;
                sequenceRunner = go.AddComponent<ActionSequence>();
            }

            sequenceRunner.remoteTarget = remoteTarget;

            isPlaying = true;

            index = 0;
            sequenceRunner.onComplete.AddListener(SequenceComplete);
            if (sequences[0] != null)
            {
                sequences[0].ApplyTo(sequenceRunner);
                sequenceRunner.Play();
            }
            else
            {
                SequenceComplete();
            }
        }

        public void Resume()
        {
            if (!isPaused) return;
            isPaused = false;
            if (sequences[index] != null)
            {
                sequences[index].ApplyTo(sequenceRunner);
                sequenceRunner.Play();
            }
            else
            {
                SequenceComplete();
            }
        }

        public void Stop()
        {
            if (!isPlaying) return;
            isPlaying = false;
            sequenceRunner.Stop();
        }

        #endregion

        #region Private Methods

        private void SequenceComplete()
        {
            if (index + 1 >= sequences.Count)
            {
                isPlaying = false;
                sequenceRunner.onComplete.RemoveListener(SequenceComplete);
                onSequenceComplete?.Invoke(index);
            }
            else
            {
                onSequenceComplete?.Invoke(index++);
                if (!isPaused)
                {
                    if (sequences[index] != null)
                    {
                        sequences[index].ApplyTo(sequenceRunner);
                        sequenceRunner.Play();
                    }
                    else
                    {
                        SequenceComplete();
                    }
                }
            }
        }

        #endregion

    }
}
