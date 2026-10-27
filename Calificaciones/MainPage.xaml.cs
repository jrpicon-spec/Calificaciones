using Calificaciones.Services;

namespace Calificaciones
{
    public partial class MainPage : ContentPage
    {
        private CancellationTokenSource? _welcomeAnimationCancellation;
        private CancellationTokenSource? _farewellAnimationCancellation;
        private bool _welcomeAnimationStarted;

        public MainPage(GradeReportAlertService gradeReportAlertService,
            BrandTransitionService brandTransitionService)
        {
            InitializeComponent();
            gradeReportAlertService.RegisterPresenter((title, message, cancel) =>
                DisplayAlertAsync(title, message, cancel));
            brandTransitionService.RegisterFarewellPresenter(PlayFarewellAsync);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_welcomeAnimationStarted)
            {
                return;
            }

            _welcomeAnimationStarted = true;
            var animationCancellation = new CancellationTokenSource();
            _welcomeAnimationCancellation = animationCancellation;
            _ = RunWelcomeAnimationAsync(animationCancellation.Token);
        }

        protected override void OnDisappearing()
        {
            _welcomeAnimationCancellation?.Cancel();
            _farewellAnimationCancellation?.Cancel();
            StopWelcomeAnimations();

            // If the page leaves before the sequence completes, reveal the app content on return.
            WelcomeOverlay.IsVisible = false;
            base.OnDisappearing();
        }

        private async Task RunWelcomeAnimationAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.WhenAll(
                    BrandMark.FadeToAsync(1, 360, Easing.CubicOut),
                    BrandMark.ScaleToAsync(1, 420, Easing.CubicOut),
                    BrandMark.TranslateToAsync(0, 0, 360, Easing.CubicOut));
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(70, cancellationToken);
                await Task.WhenAll(
                    InstituteName.FadeToAsync(1, 280, Easing.CubicOut),
                    InstituteName.TranslateToAsync(0, 0, 280, Easing.CubicOut));
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(60, cancellationToken);
                await Task.WhenAll(
                    ProductTitle.FadeToAsync(1, 300, Easing.CubicOut),
                    ProductTitle.TranslateToAsync(0, 0, 300, Easing.CubicOut));
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(50, cancellationToken);
                await Task.WhenAll(
                    Tagline.FadeToAsync(1, 250, Easing.CubicOut),
                    Tagline.TranslateToAsync(0, 0, 250, Easing.CubicOut));
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(100, cancellationToken);
                await WelcomeOverlay.FadeToAsync(0, 300, Easing.CubicInOut);
                cancellationToken.ThrowIfCancellationRequested();

                WelcomeOverlay.IsVisible = false;
            }
            catch (OperationCanceledException)
            {
                // The page is leaving; OnDisappearing stops the native animations and reveals the app.
            }
            finally
            {
                _welcomeAnimationCancellation?.Dispose();
                _welcomeAnimationCancellation = null;
            }
        }

        private void StopWelcomeAnimations()
        {
            WelcomeOverlay.CancelAnimations();
            BrandMark.CancelAnimations();
            InstituteName.CancelAnimations();
            ProductTitle.CancelAnimations();
            Tagline.CancelAnimations();
            FarewellTitle.CancelAnimations();
            FarewellMessage.CancelAnimations();
        }

        private Task PlayFarewellAsync() =>
            MainThread.InvokeOnMainThreadAsync(RunFarewellAnimationAsync);

        private async Task RunFarewellAnimationAsync()
        {
            if (_farewellAnimationCancellation is not null)
            {
                return;
            }

            var animationCancellation = new CancellationTokenSource();
            _farewellAnimationCancellation = animationCancellation;
            var cancellationToken = animationCancellation.Token;

            try
            {
                StopWelcomeAnimations();

                InstituteName.IsVisible = false;
                ProductTitle.IsVisible = false;
                Tagline.IsVisible = false;

                FarewellTitle.IsVisible = true;
                FarewellTitle.Opacity = 0;
                FarewellTitle.TranslationY = 8;
                FarewellMessage.IsVisible = true;
                FarewellMessage.Opacity = 0;
                FarewellMessage.TranslationY = 8;

                BrandMark.Opacity = 0;
                BrandMark.Scale = 0.82;
                BrandMark.TranslationY = 10;
                WelcomeOverlay.Opacity = 1;
                WelcomeOverlay.IsVisible = true;

                await Task.WhenAll(
                    BrandMark.FadeToAsync(1, 250, Easing.CubicOut),
                    BrandMark.ScaleToAsync(1, 290, Easing.CubicOut),
                    BrandMark.TranslateToAsync(0, 0, 250, Easing.CubicOut));
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(60, cancellationToken);
                await Task.WhenAll(
                    FarewellTitle.FadeToAsync(1, 260, Easing.CubicOut),
                    FarewellTitle.TranslateToAsync(0, 0, 260, Easing.CubicOut));
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(60, cancellationToken);
                await Task.WhenAll(
                    FarewellMessage.FadeToAsync(1, 260, Easing.CubicOut),
                    FarewellMessage.TranslateToAsync(0, 0, 260, Easing.CubicOut));
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(380, cancellationToken);
                await WelcomeOverlay.FadeToAsync(0, 280, Easing.CubicInOut);
                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException)
            {
                // If the app leaves this page, stop the native transition and continue to login.
            }
            finally
            {
                WelcomeOverlay.IsVisible = false;
                WelcomeOverlay.Opacity = 1;
                FarewellTitle.IsVisible = false;
                FarewellMessage.IsVisible = false;
                InstituteName.IsVisible = true;
                ProductTitle.IsVisible = true;
                Tagline.IsVisible = true;

                if (ReferenceEquals(_farewellAnimationCancellation, animationCancellation))
                {
                    _farewellAnimationCancellation = null;
                }

                animationCancellation.Dispose();
            }
        }
    }
}
