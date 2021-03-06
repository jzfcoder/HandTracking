/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Types {

    using System;

    /// <summary>
    /// ML image feature type.
    /// </summary>
    public sealed class MLImageType : MLArrayType {

        #region --Client API--
        /// <summary>
        /// Image width.
        /// </summary>
        public int width => shape[interleaved ? 2 : 3];

        /// <summary>
        /// Image height.
        /// </summary>
        public int height => shape[interleaved ? 1 : 2];

        /// <summary>
        /// Image channels.
        /// </summary>
        public int channels => shape[interleaved ? 3 : 1];

        /// <summary>
        /// Create an image feature type.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        public MLImageType (int width, int height) : this(width, height, typeof(byte)) { }

        /// <summary>
        /// Create an image feature type.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="type">Image data type.</param>
        public MLImageType (int width, int height, Type type) : this(null, type, new [] { 1, height, width, 3 }) { }

        /// <summary>
        /// Create an image feature type.
        /// </summary>
        /// <param name="name">Feature name.</param>
        /// <param name="type">Image data type.</param>
        /// <param name="shape">Image feature shape.</param>
        public MLImageType (string name, Type type, int[] shape) : base(name, type, shape) => interleaved = shape[1] > shape[3];

        /// <summary>
        /// Get the corresponding image type for a given feature type.
        /// </summary>
        /// <param name="type">Input type.</param>
        /// <returns>Corresponding image type or `null` if input type is not an image type.</returns>
        public static MLImageType FromType (in MLFeatureType type) {
            switch (type) {
                case MLImageType imageType:
                    return imageType;
                case MLArrayType arrayType when arrayType.dims == 4:
                    return new MLImageType(arrayType.name, arrayType.dataType, arrayType.shape);
                default:
                    return null;
            }
        }
        #endregion


        #region --Operations--
        private readonly bool interleaved;
        #endregion
    }
}