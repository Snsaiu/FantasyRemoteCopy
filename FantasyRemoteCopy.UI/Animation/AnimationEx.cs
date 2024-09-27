namespace FantasyRemoteCopy.UI.Animation;

public static class AnimationEx
{
    public static void OnceAninmation(this View view, TransformType transformType, double startValue, double endValue,
        uint millisecond, Easing? easing)

    {
        Task.Run(() =>
        {
            void Action()
            {
                switch (transformType)
                {
                    case TransformType.Rotate:
                        view.RotateTo(startValue, millisecond, easing);
                        break;
                    case TransformType.Scale:
                        view.ScaleTo(startValue, millisecond, easing);
                        break;
                    case TransformType.Fadein:
                        view.FadeTo(startValue, millisecond, easing);
                        break;
                    case TransformType.ReScale:
                        view.RelScaleTo(startValue, millisecond, easing);
                        break;
                    case TransformType.ReRotate:
                        view.RelRotateTo(startValue, millisecond, easing);
                        break;
                }
            }

            Application.Current!.Dispatcher.Dispatch(Action);

            Thread.Sleep((int)millisecond);

            void Action1()
            {
                switch (transformType)
                {
                    case TransformType.Rotate:
                        view.RotateTo(endValue, millisecond, easing);
                        break;
                    case TransformType.Scale:
                        view.ScaleTo(endValue, millisecond, easing);
                        break;
                    case TransformType.Fadein:
                        view.FadeTo(endValue, millisecond, easing);
                        break;
                    case TransformType.ReScale:
                        view.RelScaleTo(endValue, millisecond, easing);
                        break;
                    case TransformType.ReRotate:
                        view.RelRotateTo(endValue, millisecond, easing);
                        break;
                }
            }

            Application.Current.Dispatcher.Dispatch(Action1);
            Thread.Sleep((int)millisecond);
        });
    }

    public static void LoopAnimation(this View view, TransformType transformType, double startValue, double endValue,
        uint millisecond, Easing? easing)
    {
        var thread = new Thread(() =>
        {
            while (true)
            {
                Application.Current!.Dispatcher.Dispatch(() =>
                {
                    switch (transformType)
                    {
                        case TransformType.Rotate:
                            view.RotateTo(startValue, millisecond, easing);
                            break;
                        case TransformType.Scale:
                            view.ScaleTo(startValue, millisecond, easing);
                            break;
                        case TransformType.Fadein:
                            view.FadeTo(startValue, millisecond, easing);
                            break;
                        case TransformType.ReScale:
                            view.RelScaleTo(startValue, millisecond, easing);
                            break;
                        case TransformType.ReRotate:
                            view.RelRotateTo(startValue, millisecond, easing);
                            break;
                    }
                });
                Thread.Sleep((int)millisecond);

                Application.Current.Dispatcher.Dispatch(() =>
                {
                    switch (transformType)
                    {
                        case TransformType.Rotate:
                            view.RotateTo(endValue, millisecond, easing);
                            break;
                        case TransformType.Scale:
                            view.ScaleTo(endValue, millisecond, easing);
                            break;
                        case TransformType.Fadein:
                            view.FadeTo(endValue, millisecond, easing);
                            break;
                        case TransformType.ReScale:
                            view.RelScaleTo(endValue, millisecond, easing);
                            break;
                        case TransformType.ReRotate:
                            view.RelRotateTo(endValue, millisecond, easing);
                            break;
                    }
                });
                Thread.Sleep((int)millisecond);
            }
        });
        thread.Start();
    }
}

public enum TransformType
{
    Scale,
    Rotate,
    Transform,
    Fadein,
    ReScale,
    ReRotate
}