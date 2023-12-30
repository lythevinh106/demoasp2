using AutoMapper;
using Azure.Messaging.ServiceBus;
using demoAsp2.Data;
using demoAsp2.Dto;
using demoAsp2.Enum;
using demoAsp2.Exceptions;
using demoAsp2.Interfaces;
using demoAsp2.Models.Category.Module;
using demoAsp2.Models.CategoryDto.Category.Request;
using demoAsp2.Responsitory;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Azure;
using Serilog;
using System.Data;
using System.Text.Json;

namespace demoAsp2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]


    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly DBAspDemo2Context _context;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusClient _client2;


        private readonly BlobResponsitory _blobResponsitory;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper,

            DBAspDemo2Context context
            //, 
            //ServiceBusClient serviceBusClient
            ,

         IAzureClientFactory<ServiceBusClient> clientFactory,


              BlobResponsitory blobResponsitory,

              ILogger<CategoriesController> logger

            )
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _context = context;
            _client = clientFactory.CreateClient("client1");

            _client2 = clientFactory.CreateClient("client2");


            ///file
            _blobResponsitory = blobResponsitory;

            ///logger
            ///


            _logger = logger;


        }

        // GET: api/Categories
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Category>))]
        [ProducesResponseType(400)]

        public IActionResult Getcategories()
        {



            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
            return Ok(categories);


        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]

        public IActionResult GetCategory(int id)
        {

            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(id));

            try
            {

                if (category == null)
                {
                    return NotFound();


                }
                return Ok(category);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }



        }


        // PUT: api/Categories/5


        [HttpPut("UpdateCategoryById/{id}")]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult PutCategory(int id, [FromBody] Models.CategoryDto.Category.Request.CategoryRequestDto updatedCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                Console.WriteLine("loi request");
            }

            if (!_categoryRepository.GetCategoryExists(id))
            {
                ModelState.AddModelError("noExist", "Category not exist");

                return StatusCode(400, ModelState["noExist"].Errors);
            }

            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {


                    Console.WriteLine($"day la idddddddd  {id} va name {updatedCategory.name}");
                    bool update = _categoryRepository.UpdateCategory(id, _mapper.Map<Category>(updatedCategory));
                    if (update)
                    {

                        return Ok(updatedCategory);

                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                    transaction.Rollback();

                }
            }









            //try
            //{

            //    bool update = _categoryRepository.UpdateCategory(id, updatedCategory);
            //    Console.WriteLine("Thanh cong " + update.ToString());
            //    if (update)
            //    {
            //        return Ok();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("loi");
            //    return BadRequest(ex);

            //}
            //return NoContent();

            //if (!_categoryRepository.GetCategoryExists(updatedCategory.id))
            //{
            //    return NotFound("Value is not exist");
            //}




            //try
            //{
            //    bool updateCategory = _categoryRepository.UpdateCategory(id, updatedCategory);

            //    if (updateCategory)
            //    {
            //        return NoContent();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("", ex.ToString());
            //    return StatusCode(500, ModelState);

            //}

            return NoContent();

        }

        //POST: api/Categories

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400)]

        //[Authorize(Policy = "Premiumcontent")]

        [Authorize(Policy = "AtLeast21")]
        [Authorize(Policy = "MaleSex")]

        [Authorize(Policy = "CreateCategoryPolicy")]

        public IActionResult PostCategory(CategoryRequestDto categoryRequest)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                Console.WriteLine("loi request");
            }

            //phuong thuc nay se tu xep hang xong tu goi den de thuc thi
            /*   var jobId = BackgroundJob.Enqueue<IMail>(x => x.SendMail("day la subject2", "<h1>test body</h1>",
                   "lythevinh106@gmail.com", "lythevinh106@gmail.com", "56658sa"));*/

            //phuong thuc nay se xep hang->delay ->thực thi
            //var jobId = BackgroundJob.Schedule<IMail>(x => x.SendMail("day la subject 3s", "<h1>test body</h1>",
            //   "lythevinh106@gmail.com", "lythevinh106@gmail.com", "56658sa"), TimeSpan.FromSeconds(30));


            ///no se tu dong gui mail sau moi 1p

            //RecurringJob.AddOrUpdate<IMail>(x => x.SendMail("day la subject 3s", "<h1>test body</h1>",
            //     "lythevinh106@gmail.com", "lythevinh106@gmail.com", "56658sa"), Cron.Minutely);

            ///Continuations : thuc hien 1 cong viec sau 1 cong viec
            var jobId = BackgroundJob.Enqueue<IMail>(x => x.SendMail("day la subject2", "<h1>test body</h1>",
                   "lythevinh106@gmail.com", "lythevinh106@gmail.com", "56658sa"));

            BackgroundJob.ContinueJobWith<IMail>(jobId, (x) => x.sendMail2());




            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {




                try
                {

                    bool create = _categoryRepository.CreateCategory(_mapper.Map<Category>(categoryRequest));




                    transaction.Commit();
                    return Ok(categoryRequest);



                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.ToString());
                    return StatusCode(500, ModelState);

                }
            }

            //if (_context.categories == null)
            //{
            //    return Problem("Entity set 'DBAspDemo2Context.categories'  is null.");
            //}
            //_context.categories.Add(category);
            //_context.SaveChangesAsync();

            //return CreatedAtAction("GetCategory", new { id = category.id }, category);
        }

        [HttpPost("demoBusServiceAddCategory")]
        [Authorize]

        public async Task PostCategory2(CategoryRequestDto categoryRequest)
        {

            ///conectString

            ///day là sang sử dụng  Azure Service Bus Queue,

            var sender = _client.CreateSender("add-category");

            var body = JsonSerializer.Serialize(categoryRequest);
            var message = new ServiceBusMessage(body);
            if (body.Contains("cat"))
            {
                ///len lich de dua vao hang doi
                message.ScheduledEnqueueTime = DateTimeOffset.UtcNow.AddSeconds(10);
                Console.WriteLine("co Cat------------");
            }

            await sender.SendMessageAsync(message);



        }



        [HttpPost("demoTopicSubAddCategory")]

        public async Task PostCategory3(CategoryRequestDto categoryRequest)
        {

            ///conectString

            ///day là sang sử dụng  gui tin nhan len topic,

            var sender = _client2.CreateSender("add-topic-category");

            var body = JsonSerializer.Serialize(categoryRequest);

            var message = new ServiceBusMessage(body);

            //message.ApplicationProperties.Add("Month", categoryRequest.date.toString("MMMM"));

            ///thêm 1 property để mapping vs cái filler của subcrion
            message.ApplicationProperties.Add("name", categoryRequest.name);

            await sender.SendMessageAsync(message);



        }


        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCategory(int id)
        {

            if (!_categoryRepository.GetCategoryExists(id))
            {
                ModelState.AddModelError("noExist", "Category not exist");

                return StatusCode(400, ModelState["noExist"]!.Errors);
            }
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {

                try
                {

                    bool delete = _categoryRepository.DeleteCategory(id);




                    transaction.Commit();
                    return StatusCode(200, "Xoa thanh cong"); ;



                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.ToString());
                    return StatusCode(500, ModelState);

                }
            }
        }

        [HttpGet("getProducts/{idCategory}")]
        [Authorize]
        public IActionResult GetProductFromCategory(int idCategory)
        {


            //if (!_categoryRepository.GetCategoryExists(idCategory))
            //{
            //    ModelState.AddModelError("noExist", "Category not exist");

            //    return StatusCode(400, ModelState["noExist"]!.Errors);
            //}

            //Console.WriteLine(idCategory);
            var listProduct = _context.categories
              .Where(c => c.id == idCategory)
              .Select(c => new
              {
                  CategoryId = c.id,
                  CategoryName = c.name,
                  Products = c.products.Select(p => new { p.id, p.price, p.name })
              });





            //    day la lazyload        var cat = _context.categories.First((c) =>


            //                c.id == idCategory
            //                )

            //var listProduct = cat.products;
            //foreach (var item in listProduct)
            //{
            //    Console.WriteLine("name----------" + listProduct.);
            //}






            return Ok(listProduct.FirstOrDefault());

            //return Ok(new
            //{
            //    idCategory = listProduct.,
            //    Products = new { listProduct[0].name }


            //});














        }
        [HttpGet("getCategoriesWithProduct")]

        [Authorize(Roles = AppRole.Customer)]  ///Authorize theo role
        public IActionResult getAllCategoriesWithProdyct()
        {


            ///lay ra danh muc và cac product thuoc no
            var eagerLoading = _context.categories.Include(c => c.products)
            .Select(c => new
            {
                name = c.name,
                Products = c.products.Select(p => new
                {
                    id = p.id,
                    name = p.name,
                    price = p.price
                    // Các thuộc tính khác mà bạn muốn lấy
                })
            }).ToList();


            //su dung linq




            return Ok(eagerLoading);





        }

        [HttpGet("getOrderTotalPriceWithCategory")]


        [Authorize(Policy = "CustomerOnly")]
        public IActionResult getOrderTotalPriceWithCategory()
        {



            var result = (from c in _context.categories
                          join p in _context.products on c.id equals p.categoryId
                          join po in _context.productOrders on p.id equals po.productId
                          group new { c, p, po } by new { c.id, c.name } into grouped
                          select new
                          {
                              Id = grouped.Key.id,
                              Name = grouped.Key.name,
                              Total = grouped.Sum(x => x.p.price * x.po.quantity),
                              Products = grouped.Select(x => new
                              {
                                  ProductId = x.p.id,
                                  ProductName = x.p.name,
                                  Quantity = x.po.quantity
                              })
                          }).ToList();





            return Ok(result);














            //var options = new JsonSerializerOptions
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve
            //};

            //var jsonString = JsonSerializer.Serialize(kq, options);







        }

        [HttpGet("demoProcedures")]
        public IActionResult demoProcedures()
        {
            Console.WriteLine("day la demoProcedures");


            /* th1
              int number = 2;   
             var result = _context.Set<CategoryResponseDto>()
          .FromSqlRaw("EXEC GetProductInfo0")
           .ToList();
            */


            var sobanghiParameter = new SqlParameter("@sobanghi", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result2 = _context.products.FromSqlRaw(
            "EXEC GetProductInfo2 @productId, @sobanghi OUT",
            new SqlParameter("@productId", 2),
            sobanghiParameter).ToList();

            var sobanghiValue = (int)sobanghiParameter.Value;
            return Ok(result2);







        }

        [HttpGet("demoProcedures2")]
        public IActionResult demoProcedures2()
        {



            var result = _context.products
            .FromSqlRaw(
                "EXEC AddInsertColumn  @name, @price, @categoryId",

                new SqlParameter("@name", "sanphamdemotuasp2"),
                new SqlParameter("@price", 25632.00),
                new SqlParameter("@categoryId", 999)
            ).ToList();

            // Lấy giá trị output sau khi stored procedure thực hiện thành công


            return Ok(result.Select(p => new { id = p.id, name = p.name }));







        }



        [HttpGet("demoReciveQueue")]


        public async Task<IActionResult> demoReciveQueue()
        {
            ////tat ca cac project có thể nhận dc queue này  chỉ cần có conectionString 


            ServiceBusReceiver receiver = _client.CreateReceiver("add-category");

            // the received message is a different type as it contains some service set properties
            //nếu k xóa thì delevery count sẽ tăng
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();


            //sẽ lấy ra cái message đầu tiên trong hàng đợi
            string body = receivedMessage.Body.ToString();


            CategoryDto category = JsonSerializer.Deserialize<CategoryDto>(body);


            Console.WriteLine("ten của queue sau khi lấy" + category.name);
            /////remove queue after get it
            receiver.CompleteMessageAsync(receivedMessage);




            return Ok();
        }


        [HttpPost("demoBlobUpdate")]

        public async Task<IActionResult> demoBlobUpdate([FromForm] BlobRequest blobRequest)
        {


            await using (Stream fileStream = blobRequest.file.OpenReadStream())
            {
                var result = await _blobResponsitory.UploadBlobFile(fileStream, blobRequest.file.FileName);
                return Ok(result);
            }





        }




        [HttpGet("getListBlob")]

        public async Task<IActionResult> getListBlob()
        {




            var result = await _blobResponsitory.ListBlob();
            return Ok(result);






        }

        [HttpGet("DownLoadBlob/{blobFileName}")]

        public async Task<IActionResult> DowloadBlob(string blobFileName)
        {




            var result = await _blobResponsitory.DownLoadBlobAsync(blobFileName);


            return File(result, $"image/jpg", $"blobfile.jpg");




        }

        [HttpGet("DeleteBlob/{blobFileName}")]
        public async Task<IActionResult> DeleteBlob(string blobFileName)
        {




            await _blobResponsitory.DeleteBlobAsync(blobFileName);


            return Ok();




        }



        [HttpGet("testLogger")]
        public async Task<IActionResult> testLogger()
        {




            _logger.LogInformation("day la test loggg --info--");

            Log.Information("day la test loggg --info bang serialog");

            return Ok();




        }


        [HttpGet("demoError")]

        public IActionResult demoError()
        {

            try
            {
                throw new Exception("demo loi");//đoạn này sẽ chạy vào ex ở catch

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
                return BadRequest(ex.Message);


            }







        }

        [HttpGet("demoError2")]
        public IActionResult demoError2()
        {


            //   throw new Exception("demo loi 2");//đoạn này sẽ dc middleware bat

            throw new ExceptionBadRequest("demo loi 2");


        }




        [HttpGet("customResponse")]
        public IActionResult customResponse()
        {
            return Ok(new { message = "day la custome", data = new { data = 1 } });
        }
        // handle received messages


        //[HttpGet("demofunction")]
        //public IActionResult demofunction()
        //{

        //    //var result = _context.Database.Sql($"select dbo.CheckPrice({0})", 25);


        //    //Console.WriteLine("result" + result);
        //    //return Ok(result);




        //}



    }




}
