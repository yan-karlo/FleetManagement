import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { VehicleTypeResponseDTO } from '@models/vehicle-type/vehicleType.response.dto';
import { FormTitleComponent } from '@shared/form/form-title/form-title.component';
import { SaveDeleteTryAgainButtonsBundleComponent } from '@shared/form/save-delete-try-again-buttons-bundle/save-delete-try-again-buttons-bundle.component';
import { SpinnerComponent } from '@shared/spinner/spinner.component';
import { FormAction } from '@app/Enums/FormAction.enum';
import { LoadingStatus } from '@app/Enums/loadingStatus.enum';
import { DataService, IRequestData } from '@services/data-service/data.service';
import { CheckLoading } from '@app/utils/CheckLoading';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-vehicleTypes-form',
    imports: [
        ReactiveFormsModule,
        RouterModule,
        SpinnerComponent,
        FormTitleComponent,
        SaveDeleteTryAgainButtonsBundleComponent,
    ],
    templateUrl: './vehicle-types-form.component.html',
    styleUrl: './vehicle-types-form.component.css'
})
export class VehicleTypesFormComponent implements OnInit {
    formTempMessageTitle = 'VehicleTypes Form:'
    backToListPath = '/vehicle-types'
    vehicleType: VehicleTypeResponseDTO = new VehicleTypeResponseDTO();
    checkLoading: CheckLoading = new CheckLoading();
    vehicleTypeForm!: FormGroup;
    formAction!: string;
    isDisabled: boolean = false;
    sendRequest: any = {
        'add': this.sendCreateRequest.bind(this),
        'update': this.sendUpdateRequest.bind(this),
        'delete': this.sendDeleteRequest.bind(this),
    };

    constructor(
        private readonly route: ActivatedRoute,
        private readonly router: Router,
        private readonly dataService: DataService,
        private readonly toastr: ToastrService
    ) { }

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            var action = params.get('action') ?? "detail";
            var id = action === FormAction.add ? 0 : Number.parseInt(params.get('id') ?? '0');
            this.vehicleType.id = typeof id === 'number' ? id : -1;

            if (!(action in FormAction)) {
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse)
                throw new Error('A wrong endpoint was reached');
            }
            this.formAction = action;
        });

        this.vehicleTypeForm = this.getNewFormGroup(this.vehicleType);

        if (this.vehicleType.id > 0) {
            this.sendGetByIdRequest();
        } else if (this.formAction === FormAction.add) {
            this.checkLoading.setLoadingStatus(LoadingStatus.Successful)
        } else {
            this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse);
        }


        this.vehicleTypeForm.updateValueAndValidity();
    }

    onSubmit() {
        this.checkLoading.setLoadingStatus(LoadingStatus.RequestOngoing);
        this.sendRequest[this.formAction!]();
    }

    private getNewFormGroup(vehicleType: VehicleTypeResponseDTO): FormGroup {
        return new FormGroup({
            id: new FormControl(vehicleType.id),
            name: new FormControl(vehicleType.name, [Validators.required, Validators.maxLength(30), Validators.minLength(3)]),
            passengersCapacity: new FormControl(vehicleType.passengersCapacity, [Validators.required, Validators.max(100), Validators.min(1)])
        });
    }

    private sendGetByIdRequest() {
        this.checkLoading.setLoadingStatus(LoadingStatus.ResponseOngoing);
        const requestData: IRequestData<null> = {
            entity: 'vehicleTypes',
            action: 'get',
            body: null,
            params: `/byid/${this.vehicleType.id}`
        };
        this.dataService.run<null, VehicleTypeResponseDTO>(requestData).subscribe({
            next: (response) => {
                this.vehicleType = { ...response } as VehicleTypeResponseDTO;
                this.vehicleTypeForm = this.getNewFormGroup(this.vehicleType);
            },
            error: (error) => {
                this.toastr.error('Error when trying to load the vehicle type from database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
                if (this.formAction === FormAction.delete || this.formAction === FormAction.detail) {
                    this.vehicleTypeForm.get('name')?.disable();
                    this.vehicleTypeForm.get('passengersCapacity')?.disable();
                }
            }
        });
    }

    private sendCreateRequest() {
        Object.assign(this.vehicleType, this.vehicleTypeForm.value);
        const requestData: IRequestData<VehicleTypeResponseDTO> = {
            entity: 'vehicleTypes',
            action: 'post',
            body: this.vehicleType,
            params: ``,
        };
        this.dataService.run<VehicleTypeResponseDTO, null>(requestData).subscribe({
            next: () => {
                this.toastr.success('The vehicleType name was saved.')
            },
            error: (error) => {
                this.toastr.error('Error when trying to save a new vehicleType name in the database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulRequest);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
                this.router.navigate(['/vehicle-types']);
            }
        });
    }

    private sendUpdateRequest() {
        Object.assign(this.vehicleType, this.vehicleTypeForm.value);
        const requestData: IRequestData<VehicleTypeResponseDTO> = {
            entity: 'vehicleTypes',
            action: 'put',
            body: this.vehicleType,
            params: '',
        };
        this.dataService.run<VehicleTypeResponseDTO, null>(requestData).subscribe({
            next: () => {
                this.toastr.success('The vehicle type name was updated.')
                this.router.navigate([this.backToListPath]);
            },
            error: (error) => {
                this.toastr.error('Error when trying to save the updated vehicle type name in the database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulRequest);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
            }
        });

    }

    private sendDeleteRequest() {
        Object.assign(this.vehicleType, this.vehicleTypeForm.value);
        const requestData: IRequestData<VehicleTypeResponseDTO> = {
            entity: 'vehicleTypes',
            action: 'delete',
            body: null,
            params: `?id=${this.vehicleType.id}`,
        };
        this.dataService.run<VehicleTypeResponseDTO, null>(requestData).subscribe({
            next: () => {
                this.toastr.success('The vehicle type name was deleted.')
                this.router.navigate([this.backToListPath]);
            },
            error: (error) => {
                this.toastr.error('Error when trying to delete the actual vehicle type name in the database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulRequest);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
            }
        });

    }

    updateLoadingStatus() {
        this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
    }

    onTryAgain() {
        this.sendGetByIdRequest();
    }
}
