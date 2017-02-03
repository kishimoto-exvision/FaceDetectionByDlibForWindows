﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DlibSharp.Tests
{
    [TestFixture]
    public class FaceDetector
    {
        private ErrorCallback callback;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);

            callback = delegate(string what) { throw new Exception(what); };
            NativeMethods.dlib_set_error_redirect(callback);
        }

        [Test]
        public void RawApiNewAndDispose()
        {
            IntPtr detector = NativeMethods.dlib_get_frontal_face_detector();
            NativeMethods.dlib_frontal_face_detector_delete(detector);
        }

        /// <summary>
        /// https://github.com/davisking/dlib/blob/master/examples/face_detection_ex.cpp
        /// </summary>
        [Test]
        public void RawApiSimpleDetection()
        {
            const string imagePath = "images\\lenna.bmp";

            IntPtr detector = IntPtr.Zero;
            IntPtr image = IntPtr.Zero;
            IntPtr dets = IntPtr.Zero;
            try
            {
                detector = NativeMethods.dlib_get_frontal_face_detector();
                image = NativeMethods.dlib_array2d_uchar_new();

                NativeMethods.dlib_load_image_array2d_uchar(image, imagePath);
                NativeMethods.dlib_pyramid_up_array2d_uchar(image);

                dets = NativeMethods.vector_Rect_new1();
                NativeMethods.dlib_frontal_face_detector_operator(detector, image, 0, dets);
                unsafe
                {
                    Rect* rectangles = (Rect*)NativeMethods.vector_Rect_getPointer(dets).ToPointer();
                    long count = NativeMethods.vector_Rect_getSize(dets).ToInt64();
                    for (int i = 0; i < count; i++)
                    {
                        Console.WriteLine(rectangles[i]);
                    }
                }
            }
            finally
            {
                if (image != IntPtr.Zero)
                    NativeMethods.dlib_array2d_uchar_delete(image);
                if (detector != IntPtr.Zero)
                    NativeMethods.dlib_frontal_face_detector_delete(detector);
                if (dets != IntPtr.Zero)
                    NativeMethods.vector_Rect_delete(dets);
            }
        }

        [Test]
        public void RawApiSimpleDetectionUsingMemoryInput()
        {
            const string imagePath = "images\\lenna.bmp";
            var imageBytes = File.ReadAllBytes(imagePath);

            IntPtr detector = IntPtr.Zero;
            IntPtr image = IntPtr.Zero;
            IntPtr dets = IntPtr.Zero;
            try
            {
                detector = NativeMethods.dlib_get_frontal_face_detector();
                image = NativeMethods.dlib_array2d_uchar_new();

                NativeMethods.dlib_load_bmp_array2d_uchar(image, imageBytes, new IntPtr(imageBytes.Length));
                NativeMethods.dlib_pyramid_up_array2d_uchar(image);

                dets = NativeMethods.vector_Rect_new1();
                NativeMethods.dlib_frontal_face_detector_operator(detector, image, 0, dets);
                unsafe
                {
                    Rect* rectangles = (Rect*)NativeMethods.vector_Rect_getPointer(dets).ToPointer();
                    long count = NativeMethods.vector_Rect_getSize(dets).ToInt64();
                    for (int i = 0; i < count; i++)
                    {
                        Console.WriteLine(rectangles[i]);
                    }
                }
            }
            finally
            {
                if (image != IntPtr.Zero)
                    NativeMethods.dlib_array2d_uchar_delete(image);
                if (detector != IntPtr.Zero)
                    NativeMethods.dlib_frontal_face_detector_delete(detector);
                if (dets != IntPtr.Zero)
                    NativeMethods.vector_Rect_delete(dets);
            }
        }

        [Test]
        public void RawApiDnnMmodDetectionUsingMemoryInput()
        {
            const string imagePath = "images\\lenna.bmp";
            var imageBytes = File.ReadAllBytes(imagePath);

            IntPtr detector = IntPtr.Zero;
            IntPtr image = IntPtr.Zero;
            IntPtr dets = IntPtr.Zero;
            try
            {
                detector = NativeMethods.dlib_dnn_mmod_face_detection_construct();
                image = NativeMethods.dlib_array2d_uchar_new();

                NativeMethods.dlib_load_bmp_array2d_uchar(image, imageBytes, new IntPtr(imageBytes.Length));
                NativeMethods.dlib_pyramid_up_array2d_uchar(image);

                dets = NativeMethods.vector_Rect_new1();
                NativeMethods.dlib_dnn_mmod_face_detection_operator(detector, image, dets);
                unsafe
                {
                    Rect* rectangles = (Rect*)NativeMethods.vector_Rect_getPointer(dets).ToPointer();
                    long count = NativeMethods.vector_Rect_getSize(dets).ToInt64();
                    for (int i = 0; i < count; i++)
                    {
                        Console.WriteLine(rectangles[i]);
                    }
                }
            }
            finally
            {
                if (image != IntPtr.Zero)
                    NativeMethods.dlib_array2d_uchar_delete(image);
                if (detector != IntPtr.Zero)
                    NativeMethods.dlib_frontal_face_detector_delete(detector);
                if (dets != IntPtr.Zero)
                    NativeMethods.vector_Rect_delete(dets);
            }
        }
    }
}