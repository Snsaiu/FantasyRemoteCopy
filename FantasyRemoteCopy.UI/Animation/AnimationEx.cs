namespace FantasyRemoteCopy.UI.Animation;

public static class AnimationEx
{

    public static void OnceAninmation(this View view, TransformType transformType, double startValue, double endValue,
        uint millisecond, Easing? easing)

    {
        Task.Run(() =>
        {

          

                Application.Current.Dispatcher.Dispatch(async () =>
                {
                    switch (transformType)
                    {
                        case TransformType.Rotate:
                            await view.RotateTo(startValue, length: millisecond, easing);
                            break;
                        case TransformType.Scale:
                            await view.ScaleTo(startValue, length: millisecond, easing);
                            break;
                        case TransformType.Fadein:
                            await view.FadeTo(startValue, length: millisecond, easing);
                            break;
                        case TransformType.ReScale:
                            await view.RelScaleTo(startValue, length: millisecond, easing);
                            break;
                        case TransformType.ReRotate:
                            await view.RelRotateTo(startValue, length: millisecond, easing);
                            break;
                        default:
                            break;

                    }


                });

                Thread.Sleep((int)millisecond);

                Application.Current.Dispatcher.Dispatch(async () =>
                {

                    switch (transformType)
                    {
                        case TransformType.Rotate:
                            await view.RotateTo(endValue, length: millisecond, easing);
                            break;
                        case TransformType.Scale:
                            await view.ScaleTo(endValue, length: millisecond, easing);
                            break;
                        case TransformType.Fadein:
                            await view.FadeTo(endValue, length: millisecond, easing);
                            break;
                        case TransformType.ReScale:
                            await view.RelScaleTo(endValue, length: millisecond, easing);
                            break;
                        case TransformType.ReRotate:
                            await view.RelRotateTo(endValue, length: millisecond, easing);
                            break;
                        default:
                            break;

                    }

                });
                Thread.Sleep((int)millisecond);
            

        });
    }
    public static void LoopAnimation(this View  view, TransformType transformType, double startValue,double endValue, uint millisecond,Easing? easing)
    {
        Task.Run(() =>
        {

            while (true)
            {

                Application.Current.Dispatcher.Dispatch(async () =>
                {
                    switch (transformType)
                    {
                        case TransformType.Rotate:
                            await view.RotateTo(startValue, length: millisecond,easing);
                            break;
                        case TransformType.Scale:
                            await view.ScaleTo(startValue, length: millisecond,easing);
                            break;
                        case TransformType.Fadein:
                            await view.FadeTo(startValue, length: millisecond, easing);
                            break;
                        case TransformType.ReScale:
                            await view.RelScaleTo(startValue, length: millisecond, easing);
                            break;
                        case TransformType.ReRotate:
                            await view.RelRotateTo(startValue, length: millisecond, easing);
                            break;
                        default:
                            break;

                    }

                   
                });

                Thread.Sleep((int)millisecond);

                Application.Current.Dispatcher.Dispatch(async () =>
                {

                    switch (transformType)
                    {
                        case TransformType.Rotate:
                            await view.RotateTo(endValue, length: millisecond, easing);
                            break;
                        case TransformType.Scale:
                            await view.ScaleTo(endValue, length: millisecond, easing);
                            break;
                        case TransformType.Fadein:
                            await view.FadeTo(endValue, length: millisecond, easing);
                            break;
                        case TransformType.ReScale:
                            await view.RelScaleTo(endValue, length: millisecond, easing);
                            break;
                        case TransformType.ReRotate:
                            await view.RelRotateTo(endValue, length: millisecond, easing);
                            break;
                        default:
                            break;

                    }

                });
                Thread.Sleep((int)millisecond);
            }

        });
    }
}

public enum TransformType
{
    Scale,
    Rotate,
    Transform,
    Fadein,
    ReScale,
    ReRotate,
}