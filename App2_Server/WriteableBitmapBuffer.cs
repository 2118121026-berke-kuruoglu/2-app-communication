using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public class WriteableBitmapBuffer
{
    public int Width { get; }
    public int Height { get; }
    public int Stride { get; }
    public IntPtr Data { get; }
    private byte[] buffer;

    public WriteableBitmapBuffer(int width, int height)
    {
        Width = width;
        Height = height;
        Stride = width * 4;
        buffer = new byte[Stride * height];
        Data = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
    }

    public Bitmap ToBitmap()
    {
        Bitmap bmp = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);
        BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
        Marshal.Copy(buffer, 0, bmpData.Scan0, buffer.Length);
        bmp.UnlockBits(bmpData);
        return bmp;
    }
}