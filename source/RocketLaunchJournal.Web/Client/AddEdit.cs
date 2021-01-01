using Blazored.Modal;
using Blazored.Modal.Services;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Web.Client.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Client
{
    public static class AddEdit
    {
        public static async Task AddEditRocketAsync(IModalService modalService, SmartishTable.Root<RocketDto> table, int index)
        {
            if (table == null || table.SafeList == null)
                return;

            var isNew = index == -1;
            RocketDto? dto = null;
            if (!isNew)
                dto = table.GetAt(index);

            var parameters = new Blazored.Modal.ModalParameters();
            parameters.Add(nameof(RocketAddEdit.DtoOriginal), dto);
            var modalReference = modalService.Show<RocketAddEdit>("Rocket Add/Edit", parameters, new ModalOptions() { });
            var modalResult = await modalReference.Result;
            dto = modalResult.Data as RocketDto;
            if (dto != null)
            {
                if (isNew)
                    await table.Add(dto);
                else
                    await table.UpdateAt(index, dto);
            }
        }

        public static async Task AddEditLaunchAsync(IModalService modalService, SmartishTable.Root<LaunchDto> table, int index, RocketDto? rocketDto = null)
        {
            if (table == null || table.SafeList == null)
                return;
            
            var isNew = index == -1;
            LaunchDto? dto = null;
            if (!isNew)
                dto = table.GetAt(index);
            var parameters = new Blazored.Modal.ModalParameters();
            parameters.Add(nameof(LaunchAddEdit.DtoOriginal), dto);
            if (rocketDto != null)
                parameters.Add(nameof(LaunchAddEdit.SelectedRocketId), rocketDto.RocketId);
            parameters.Add(nameof(LaunchAddEdit.InitialLaunchNumber), (rocketDto?.NumberOfLaunches ?? 0) + 1);
            var modalReference = modalService.Show<LaunchAddEdit>("Launch Add/Edit", parameters, new ModalOptions() { });
            var modalResult = await modalReference.Result;
            dto = modalResult.Data as LaunchDto;
            if (dto != null)
            {
                if (isNew)
                    await table.Add(dto);
                else
                    await table.UpdateAt(index, dto);

                if (rocketDto != null && dto.LaunchNumber > rocketDto.NumberOfLaunches)
                    rocketDto.NumberOfLaunches = dto.LaunchNumber;
            }
        }
    }
}
