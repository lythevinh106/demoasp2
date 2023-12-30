using demoAsp2.Exceptions;
using demoAsp2.Models;

namespace demoAsp2.MiddleWare
{
    public class ExceptionHanderlingMiddlerWare
    {

        private readonly RequestDelegate next;

        public ExceptionHanderlingMiddlerWare(RequestDelegate next) => this.next = next;
        public async Task InvokeAsync(HttpContext context)
        {


            try
            {
                await next(context);

            }

            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";


                var (code, message) = HandleException(context, ex);
                context.Response.StatusCode = code;

                var errorResponse = new ErrorResponse
                {
                    StatusCode = code,
                    Message = ex.Message
                };



                context.Response.WriteAsync(errorResponse.ToString());


            }




        }

        private (int, string) HandleException(HttpContext context, Exception ex)
        {
            int statusCode = StatusCodes.Status500InternalServerError;
            switch (ex)
            {
                case ExceptionNotFound:
                    statusCode = StatusCodes.Status404NotFound;
                    break;


                case ExceptionBadRequest:
                    statusCode = StatusCodes.Status400BadRequest;
                    break;



            }
            return (statusCode, ex.Message);



        }
    }
}

/*Cac lỗi thường thấy trong asp
 * 
 
ASP.NET Core 6 cung cấp một số phương thức thuận tiện để trả về các lỗi thông qua HTTP response. Dưới đây là một số trong số đó:

--NotFound(): Trả về mã lỗi 404 Not Found. 404 
+tài nguyên k tồn tại,id k tồn tại khi truy vấn

return NotFound();





--BadRequest(): Trả về mã lỗi 400 Bad Request.
+Khi client yêu cầu một phương thức HTTP không được hỗ trợ bởi endpoint hoặc server. Ví dụ:

http
Copy code
// Yêu cầu DELETE tới endpoint chỉ hỗ trợ GET và POST
DELETE /api/posts

+khi gui thieu du lieu trong body khi post chẳng hạn, or dữ liệu sai định dạng mà sever k hiểu

return BadRequest();





Unauthorized(): Trả về mã lỗi 401 Unauthorized.
+chưa đăng nhập
return Unauthorized();







Forbid(): Trả về mã lỗi 403 Forbidden.
-khi không đủ thầm quyền truy cập vào  tài nguyên
return Forbid();





StatusCode(): Trả về mã lỗi HTTP bất kỳ.

  return StatusCode(418, "I'm a teapot");




Problem(): Trả về một đối tượng ProblemDetails.

return Problem(); 
 
 
 
 
 */

/*trả về dữ liệu khi thành công
  
 Ok.
 
 
 */
