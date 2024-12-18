import { CheckLoading } from '@app/utils/CheckLoading';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { VehicleResponseDTO } from '@models/vehicle/vehicle.response.dto';
import { DataService, IRequestData } from '@services/data-service/data.service';
import { ToastrService } from 'ngx-toastr';

import { LoadingStatus } from '@app/Enums/loadingStatus.enum';
import { SpinnerComponent } from '@app/components/shared/spinner/spinner.component';

@Component({
    selector: 'app-vehicles-list',
    imports: [RouterModule, SpinnerComponent],
    templateUrl: './vehicles-list.component.html',
    styleUrl: './vehicles-list.component.css',
    providers: [
        DataService,
    ]
})
export class VehiclesListComponent {
    checkLoading: CheckLoading = new CheckLoading();
    data: any[] = [];

    constructor(private dataService: DataService, private toastr: ToastrService) { }


    ngOnInit(): void {
        const requestData: IRequestData<any> = {
            entity: 'vehicles',
            action: 'get',
            body: null,
            params: undefined
        };
        this.dataService.run<any, VehicleResponseDTO[]>(requestData).subscribe({
            next: (response) => {
                this.data = [...response];
            },
            error: (error) => {
                this.toastr.error('Error when performing the request: \n' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
            }
        });
    }
}
