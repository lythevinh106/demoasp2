using Microsoft.AspNetCore.Authorization;

namespace demoAsp2.Authorize
{
    public class permissonCrud : IAuthorizationRequirement
    {
        public string Module { get; } // Ví dụ: Thuộc tính để xác định module
        public string Action { get; } // Ví dụ: Thuộc tính để xác định hành động (delete, update, create, ...)

        public permissonCrud(string module, string action)
        {
            Module = module;
            Action = action;
        }
    }
}
