import { CheckLoading } from '@app/utils/CheckLoading';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { VehicleTypeResponseDTO } from '@models/vehicle-type/vehicleType.response.dto';
import { DataService, IRequestData } from '@services/data-service/data.service';
import { ToastrService } from 'ngx-toastr';

import { LoadingStatus } from '@app/Enums/loadingStatus.enum';
import { SpinnerComponent } from '@app/components/shared/spinner/spinner.component';

@Component({
    selector: 'app-vehicle-types-list',
    imports: [RouterModule, SpinnerComponent],
    templateUrl: './vehicle-types-list.component.html',
    styleUrl: './vehicle-types-list.component.css',
    providers: [
        DataService,
    ]
})
export class VehicleTypesListComponent {
    checkLoading: CheckLoading = new CheckLoading();
    data: any[] = [];

    constructor(private dataService: DataService, private toastr: ToastrService) { }


    ngOnInit(): void {
        const requestData: IRequestData<any> = {
            entity: 'vehicleTypes',
            action: 'get',
            body: null,
            params: undefined
        };
        this.dataService.run<any, VehicleTypeResponseDTO[]>(requestData).subscribe({
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
