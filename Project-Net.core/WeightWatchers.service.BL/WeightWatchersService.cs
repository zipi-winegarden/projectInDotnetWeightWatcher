using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weight_Watchers.core.interfaces_DAL;
using Weight_Watchers.core.interfaces_Service;
using Weight_Watchers.core.Response;
using Weight_Watchers.core.DTO;
using Weight_Watchers.data.Entities;

namespace Weight_Watchers.service.BL
{
    public class WeightWatchersService : IWeightWatcherService
    {
        readonly IMapper _mapper;
        readonly IWeightWatchersRepository _weightWatchersRepository;
        public WeightWatchersService(IMapper mapper, IWeightWatchersRepository weightWatchersRepository)
        {
            _mapper = mapper;
            _weightWatchersRepository = weightWatchersRepository;
        }
        public async Task<BaseResponseGeneral<SubscriberAndCard>> GetById(int id)
        {
            BaseResponseGeneral<SubscriberAndCard> response = new BaseResponseGeneral<SubscriberAndCard>();
            //אם אין כזה כרטיס
            if (!_weightWatchersRepository.IsCardExist(id))
            {
                response.Succeed = false;
                response.Message = "could not find card with such id.";
            }
            else
            {
                response = await _weightWatchersRepository.GetById(id);
            }
            return response;
        }

        public async Task<int?> Login(string email,string password )
        {
            if (!IsValidEmail(email))
                return null;
            if (!_weightWatchersRepository.IsSubscriberExist(email))     
                return null; ///אין כזה בן אדם!!!   
            if (!IsValdPassword(password))
                return null;
            return await _weightWatchersRepository.Login(email,password);

        }

        public async Task<BaseResponse> Register(SubscriberDTO s1)
        { 
            BaseResponse b= new BaseResponse();
            if (_weightWatchersRepository.IsSubscriberExist(s1.Email))
            {
                b.Succeed = false;
                b.Message = "subscriber already exists!";
                return b;
            }
            return await _weightWatchersRepository.Register(_mapper.Map<Subscriber>(s1), s1.Height);


        }
        public bool IsValidEmail(string email)
        {
            return true;
        }
        public bool IsValdPassword(string password)
        {
            return true;

        }
    }
}
