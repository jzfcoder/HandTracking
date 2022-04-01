
// the true predictor being called from HandPoseSample
// A modified script of Yusuf Olokoba's original (creator of NatML)
// Handles all NatML predictions

namespace NatSuite.ML.Vision {

    using System;
    using System.Runtime.InteropServices;
    using Internal;
    using Types;

    /// <summary>
    /// Hand pose predictor.
    /// This predictor only supports predicting a single hand.
    /// </summary>
    public sealed partial class HandPosePredictor : IMLPredictor<HandPosePredictor.Hand> { // DEPLOY

        #region --Client API--
        /// <summary>
        /// Create a hand pose predictor.
        /// </summary>
        /// <param name="model">Hand landmark ML model.</param>
        /// <param name="minScore">Minimum confidence score.</param>
        public HandPosePredictor (MLModel model, float minScore = 0.6f) {
            this.model = model;
            this.minScore = minScore;
        }

        /// <summary>
        /// Detect hand pose in an image.
        /// </summary>
        /// <param name="inputs">Input image.</param>
        /// <returns>Detected hand or `null` if the confidence score is too low.</returns>
        public unsafe Hand Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"Hand landmark predictor expects a single feature", nameof(inputs));
            // Check type
            var input = inputs[0];
            if (!(input.type is MLArrayType type))
                throw new ArgumentException(@"Hand landmark predictor expects an an array or image feature", nameof(inputs));  
            // Predict
            var inputType = model.inputs[0] as MLImageType;
            var inputFeature = (input as IMLFeature).Create(inputType);
            var outputFeatures = model.Predict(inputFeature);
            inputFeature.ReleaseFeature();
            // Marshal
            var score = *(float*)outputFeatures[0].FeatureData();
            var handedness = *(float*)outputFeatures[1].FeatureData();
            var anchors = new float[63];
            Marshal.Copy(outputFeatures[2].FeatureData(), anchors, 0, anchors.Length);
            foreach (var feature in outputFeatures)
                feature.ReleaseFeature();
            // Check
            if (score < minScore)
                return default;
            // Return
            var result = new Hand(anchors, score, handedness, inputType.height);
            return result;
        }
        #endregion


        #region --Operations--
        private readonly IMLModel model;
        private readonly float minScore;

        void IDisposable.Dispose () { } // Not used
        #endregion
    }
}