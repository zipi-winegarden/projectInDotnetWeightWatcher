using Microsoft.AspNetCore.Mvc;
using Weight_Watchers.core.DTO;
using Weight_Watchers.core.interfaces_Service;
using Weight_Watchers.core.Response;
using Weight_Watchers.data.Entities;

namespace Project_Net.core.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class weightWatchersController:ControllerBase
    {
        readonly IWeightWatcherService _weightWatcherService;
        public weightWatchersController(IWeightWatcherService weightWatcherService)
        {
            _weightWatcherService= weightWatcherService;
        }
        //POST -- LOGIN
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<BaseResponseGeneral<int?>>> Login(string email,string password)
        {
            BaseResponseGeneral<int?> response = new BaseResponseGeneral<int?>();
            response.Data=  await _weightWatcherService.Login(email, password);
            if (response.Data == null)
                return   Unauthorized(response);
            return Ok(response);

        }

        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult<BaseResponseGeneral<SubscriberAndCard>>> GetById(int id)
        {
            BaseResponseGeneral<SubscriberAndCard> response = new BaseResponseGeneral<SubscriberAndCard>();
            response= await _weightWatcherService.GetById(id);
            if (response.Succeed==false)
                return NotFound(response);
            return Ok(response.Data);
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<BaseResponse>> Register(SubscriberDTO sDto)
        {
            BaseResponse br = await _weightWatcherService.Register(sDto);

            if(br==null)
                return Conflict(br);

            return Ok(br);
        }

    }
}
