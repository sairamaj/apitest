using ApiViewer.Model;

namespace ApiViewer.ViewModel
{
    internal class ApiInfoViewModel
    {
        public ApiInfoViewModel(ApiInfo apiInfo)
        {
            ApiInfo = apiInfo;
        }
        public ApiInfo ApiInfo { get; }
    }
}
