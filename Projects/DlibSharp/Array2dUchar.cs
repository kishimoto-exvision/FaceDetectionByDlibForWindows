﻿namespace DlibSharp
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Diagnostics;

    public class Array2dUchar : IDisposable
    {
        internal IntPtr DlibArray2dUchar { get; private set; }

        public Array2dUchar()
        {
            DlibArray2dUchar = NativeMethods.dlib_array2d_uchar_new();
        }

        public Int32 Width { get { return NativeMethods.dlib_array2d_uchar_nc(DlibArray2dUchar); } }
        public Int32 Height { get { return NativeMethods.dlib_array2d_uchar_nr(DlibArray2dUchar); } }

        public void SetBitmap(System.Drawing.Bitmap inputImage)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                inputImage.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] imageBytes = stream.ToArray();
                NativeMethods.dlib_load_bmp_array2d_uchar(DlibArray2dUchar, imageBytes, new IntPtr(imageBytes.Length));
            }
        }

        public void PyramidUp()
        {
            Trace.Assert(DlibArray2dUchar != IntPtr.Zero);
            NativeMethods.dlib_pyramid_up_array2d_uchar(DlibArray2dUchar);
        }

        #region IDisposable
        private bool disposed = false;
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) { return; }
            if (disposing)
            {
                // dispose managed objects, and dispose objects that implement IDisposable
            }
            // release any unmanaged objects and set the object references to null
            if (DlibArray2dUchar != IntPtr.Zero) { NativeMethods.dlib_array2d_uchar_delete(DlibArray2dUchar); DlibArray2dUchar = IntPtr.Zero; }
            disposed = true;
        }
        ~Array2dUchar() { Dispose(false); }
        #endregion
    }

    [SuppressUnmanagedCodeSecurity]
    internal static partial class NativeMethods
    {
        [DllImport(DlibExternDllPath, CallingConvention = CallingConvention.Cdecl)]
        extern internal static IntPtr dlib_array2d_uchar_new();

        [DllImport(DlibExternDllPath, CallingConvention = CallingConvention.Cdecl)]
        extern internal static void dlib_array2d_uchar_delete(IntPtr obj);

        [DllImport(DlibExternDllPath, CallingConvention = CallingConvention.Cdecl)]
        extern internal static Int32 dlib_array2d_uchar_nr(IntPtr obj);

        [DllImport(DlibExternDllPath, CallingConvention = CallingConvention.Cdecl)]
        extern internal static Int32 dlib_array2d_uchar_nc(IntPtr obj);

        [DllImport(DlibExternDllPath, CallingConvention = CallingConvention.Cdecl)]
        extern internal static void dlib_load_image_array2d_uchar(IntPtr obj, string file_name);

        [DllImport(DlibExternDllPath, CallingConvention = CallingConvention.Cdecl)]
        extern internal static void dlib_load_bmp_array2d_uchar(IntPtr obj, [MarshalAs(UnmanagedType.LPArray)] byte[] buffer, IntPtr buffer_length);

        [DllImport(DlibExternDllPath, CallingConvention = CallingConvention.Cdecl)]
        extern internal static void dlib_pyramid_up_array2d_uchar(IntPtr obj);
    }
}