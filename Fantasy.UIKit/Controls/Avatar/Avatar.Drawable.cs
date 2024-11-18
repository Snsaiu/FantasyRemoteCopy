using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

#if WINDOWS
using Microsoft.UI.Xaml.Media.Imaging;

using Windows.Storage.Streams;

#elif MACCATALYST
#elif ANDROID
#else
#endif

namespace Fantasy.UIKit;

public partial class Avatar
{
    private Uri? backUri;

    private IImage? image;

    public override async void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.GetClipPath(dirtyRect));
        if (Source is null)
        {
            canvas.DrawBackground(this, dirtyRect);
        }
        else
        {
#if WINDOWS
            var imageSourceServiceProvider =
                Handler.MauiContext.Services.GetRequiredService<IImageSourceServiceProvider>();

            var imageSourceService = imageSourceServiceProvider.GetImageSourceService(Source);
            if (imageSourceService is null)
                throw new NullReferenceException();

            if ((await imageSourceService.GetImageSourceAsync(Source)).Value is not BitmapImage bitmapImage)
                throw new NullReferenceException();

            if (backUri is null || bitmapImage.UriSource.AbsolutePath != backUri.AbsolutePath)
            {
                backUri = bitmapImage.UriSource;
                var streamReference =
                    RandomAccessStreamReference.CreateFromUri(backUri);
                using var streamReader = await streamReference.OpenReadAsync();

                var stream = streamReader.AsStreamForRead();
                image = PlatformImage.FromStream(stream);
            }
            else if (backUri is null)
            {
                image = null;
            }

            if (image is not null)
                canvas.DrawImage(image, dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);


#endif

            var imageSourceServiceProvider =
                Handler.MauiContext.Services.GetRequiredService<IImageSourceServiceProvider>();

            if (imageSourceServiceProvider is null)
                throw new NullReferenceException();

            var imageSourceService = imageSourceServiceProvider.GetImageSourceService(Source);
            if (imageSourceService is null)
                throw new NullReferenceException();

            var uiimage = await imageSourceService.GetPlatformImageAsync(Source, Handler.MauiContext);
            if (uiimage is null)
                throw new NullReferenceException();

            var stream = (uiimage.Value.AsPNG()?.AsStream()) ?? throw new NullReferenceException();
            image = PlatformImage.FromStream(stream);
            canvas.DrawImage(image, dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);

        }

        if (Enable == false)
        {
            canvas.FillColor =
                Colors.White.MultiplyAlpha(0.12f);
            canvas.FillRectangle(dirtyRect);
        }


        canvas.DrawBorderColor(this, dirtyRect);
        canvas.ResetState();
    }
}