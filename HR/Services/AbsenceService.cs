using HR.DTO.Inbound;
using HR.DTO;
using HR.Services.Interfaces;
using HR.UOW.Interfaces;
using HR.Repository;
using Models;
using System;
using Mapster;

namespace HR.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly IMainUOW _mainUOW;
        private readonly IConfiguration _configuration;

        public AbsenceService(IMainUOW mainUOW, IConfiguration configuration)
        {
            _mainUOW = mainUOW;
            _configuration = configuration;
        }

        public async Task<AbsenceDto> AddOrRequestAbsence(AddAbsenceRequest absenceRequest, int myId, int companyId, int userRole)
        {
            DateOnly startDate = DateOnly.FromDateTime(absenceRequest.StartDateTime);
            DateOnly endDate = DateOnly.FromDateTime(absenceRequest.EndDateTime);

            TimeOnly startTime = TimeOnly.FromDateTime(absenceRequest.StartDateTime);
            TimeOnly endTime = TimeOnly.FromDateTime(absenceRequest.EndDateTime);

            //throw new Exception("Check if ContractRepo.GetContractsThatFallBetween() is in use by another method, if not then change it, else create a new method to get contracts to add the absence.");
            List<Contract> contracts = await _mainUOW.ContractRepo.GetContractsBetweenDates(startDate, endDate, absenceRequest.EmployeeId, companyId);

            if (contracts.Count == 0) throw new Exception("User has no contract");
            if (contracts.Count > 1) throw new Exception("Can not add absence between 2 contracts");

            int requestAddOrError = await _mainUOW.HierarchyRepo.ValidateRequestOrAddAbsence(myId, userRole, absenceRequest.EmployeeId);

            var addedAbsence = new Absence();

            // -1 error
            // 0 request
            // 1 add
            switch (requestAddOrError)
            {
                case 0: addedAbsence = await _mainUOW.AbsenceRepo.AddOrRequestAbsence(absenceRequest, startDate, endDate, startTime, endTime, myId, companyId, false);
                    break;
                case 1: addedAbsence = await _mainUOW.AbsenceRepo.AddOrRequestAbsence(absenceRequest, startDate, endDate, startTime, endTime, myId, companyId, true);
                    break;
                case -1: throw new Exception("Your permissions are not sufficiant for the operation.");
                default: throw new Exception("An unexpected error has occured while adding the Absence.");
            }

            var absenceDto = addedAbsence.Adapt<AbsenceDto>();

            return absenceDto;
        }
    }
}
