import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ColorResponseDTO } from '@models/color/color.response.dto';
import { CheckLoading } from '@app/utils/CheckLoading';
import { DataService, IRequestData } from '@services/data-service/data.service';
import { ToastrService } from 'ngx-toastr';

import { LoadingStatus } from '@app/Enums/loadingStatus.enum';
import { SpinnerComponent } from '@shared/spinner/spinner.component';

@Component({
    selector: 'app-colors-list',
    imports: [RouterModule, SpinnerComponent],
    templateUrl: './colors-list.component.html',
    styleUrl: './colors-list.component.css',
    providers: [
        DataService,
    ]
})
export class ColorsListComponent {
    checkLoading: CheckLoading = new CheckLoading();
    data: any[] = [];

    constructor(private dataService: DataService, private toastr: ToastrService) { }


    ngOnInit(): void {
        const requestData: IRequestData<any> = {
            entity: 'colors',
            action: 'get',
            body: null,
            params: undefined
        };
        this.dataService.run<any, ColorResponseDTO[]>(requestData).subscribe({
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
