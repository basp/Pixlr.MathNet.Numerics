## Pixlr for MathNet.Numerics
This is **Pixlr** with a full dependency on the awesome **MathNet.Numerics** library. The main difference is that we are using using the `Vector<T>` and `Matrix<T>` classes from **MathNet.Numerics.LinearAlgebra** instead of the builtin classes. The obvious benefit is that they are much more solid and useful. The drawback is that we can't easily tweak them in case they won't suit our needs.

### overview
The main goal of **Pixlr** is to do (reasonably) fast `Bitmap` manipulations in **.NET**. Using `GetPixel` and `SetPixel` is extremely slow so the first thing we do is to provide a **disposable** wrapper over the locked bitmap data in the form of the `LockedBitmapData` class.

Once you have an instance of this class you get some useful methods to either do some manipulations in place or convert it to a `Matrix<T>` if you really want to do some heavy lifting.

Once you're done with the calculations you can easily convert it back into a `Bitmap` instance for display purposes as well.

```
var bmp = (Bitmap)Bitmap.FromFile(@"/path/to/bmp");
using(var data = bmp.Lock())
{
    // ...
}
```