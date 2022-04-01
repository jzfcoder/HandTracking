
// A modified class of Yusuf Olokoba (creator of NatML)
// Creates an object called 'Hand' to store all of its information (handedness, locations, orientation, confidence, etc)

namespace NatSuite.ML.Vision {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed partial class HandPosePredictor {

        /// <summary>
        /// Handedness.
        /// </summary>
        public enum Handedness {
            Left = 0,
            Right = 1
        }

        /// <summary>
        /// Detected hand.
        /// </summary>
        public sealed class Hand : IReadOnlyList<Vector3> {

            #region --Client API--
            public readonly float score;
            public readonly Handedness handedness;
            public Vector3 this [int idx] => new Vector3(data[idx * 3 + 0], height - data[idx * 3 + 1], data[idx * 3 + 2]);
            public Vector3 wrist => this[0];
            public Vector3 thumbCMC => this[1];
            public Vector3 thumbMCP => this[2];
            public Vector3 thumbIP => this[3];
            public Vector3 thumbTip => this[4];
            public Vector3 indexMCP => this[5];
            public Vector3 indexPIP => this[6];
            public Vector3 indexDIP => this[7];
            public Vector3 indexTip => this[8];
            public Vector3 middleMCP => this[9];
            public Vector3 middlePIP => this[10];
            public Vector3 middleDIP => this[11];
            public Vector3 middleTip => this[12];
            public Vector3 ringMCP => this[13];
            public Vector3 ringPIP => this[14];
            public Vector3 ringDIP => this[15];
            public Vector3 ringTip => this[16];
            public Vector3 pinkyMCP => this[17];
            public Vector3 pinkyPIP => this[18];
            public Vector3 pinkyDIP => this[19];
            public Vector3 pinkyTip => this[20];
            #endregion


            #region --Operations--

            private readonly float[] data;
            private readonly int height;

            internal Hand (float[] data, float score, float handedness, int height) {
                this.data = data;
                this.score = score;
                this.handedness = handedness >= 0.5f ? Handedness.Right : Handedness.Left;
                this.height = height;
            }

            int IReadOnlyCollection<Vector3>.Count => data.Length / 3;

            IEnumerator<Vector3> IEnumerable<Vector3>.GetEnumerator () {
                var count = (this as IReadOnlyCollection<Vector3>).Count;
                for (var i = 0; i < count; ++i)
                    yield return this[i];
            }

            IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable<Vector3>).GetEnumerator();
            #endregion
        }
    }
}