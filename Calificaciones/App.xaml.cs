using Calificaciones.Services;

namespace Calificaciones
{
    public partial class App : Application
    {
        private readonly GradeReportAlertService _gradeReportAlertService;
        private readonly BrandTransitionService _brandTransitionService;

        public App(GradeReportAlertService gradeReportAlertService,
            BrandTransitionService brandTransitionService)
        {
            _gradeReportAlertService = gradeReportAlertService;
            _brandTransitionService = brandTransitionService;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage(_gradeReportAlertService, _brandTransitionService))
            {
                Title = "Sudamericano Grade"
            };
        }
    }
}
