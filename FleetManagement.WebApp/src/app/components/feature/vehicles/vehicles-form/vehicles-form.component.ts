import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, MaxLengthValidator, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { VehicleResponseDTO } from '@models/vehicle/vehicle.response.dto';
import { FormTitleComponent } from '@shared/form/form-title/form-title.component';
import { SaveDeleteTryAgainButtonsBundleComponent } from '@shared/form/save-delete-try-again-buttons-bundle/save-delete-try-again-buttons-bundle.component';
import { SpinnerComponent } from '@shared/spinner/spinner.component';
import { FormAction } from '@app/Enums/FormAction.enum';
import { LoadingStatus } from '@app/Enums/loadingStatus.enum';
import { DataService, IRequestData } from '@services/data-service/data.service';
import { CheckLoading } from '@app/utils/CheckLoading';
import { ToastrService } from 'ngx-toastr';
import { ColorResponseDTO } from '@app/models/color/color.response.dto';
import { VehicleTypeResponseDTO } from '@app/models/vehicle-type/vehicleType.response.dto';
import { VehicleRequestDTO } from '@app/models/vehicle/vehicle.request.dto';

@Component({
    selector: 'app-vehicles-form',
    imports: [
        ReactiveFormsModule,
        RouterModule,
        SpinnerComponent,
        FormTitleComponent,
        SaveDeleteTryAgainButtonsBundleComponent,
    ],
    templateUrl: './vehicles-form.component.html',
    styleUrl: './vehicles-form.component.css'
})
export class VehiclesFormComponent implements OnInit {
    formTempMessageTitle = 'Vehicles Form:'
    backToListPath = '/vehicles'
    vehicle: VehicleResponseDTO = new VehicleResponseDTO();
    colors: ColorResponseDTO[] = [];
    vehicleTypes: VehicleTypeResponseDTO[] = [];
    checkLoading: CheckLoading = new CheckLoading();
    vehicleForm!: FormGroup;
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

    async ngOnInit(): Promise<void> {
        this.route.paramMap.subscribe(params => {
            this.formAction = params.get('action') ?? "detail";
            var id = this.formAction === FormAction.add ? 0 : Number.parseInt(params.get('id') ?? '0');
            this.vehicle.id = typeof id === 'number' ? id : -1;

            if (!(this.formAction in FormAction)) {
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse);
                throw new Error('A wrong endpoint was reached');
            }
        });

        this.isDisabled = this.formAction === 'detail' || this.formAction === 'delete';

        if (this.vehicle.id > 0) {
            await this.sendGetByIdRequest();
            this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
        } else if (this.formAction === FormAction.add) {
            await this.getAuxiliaryData();
            this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
        } else {
            this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse);
        }

        this.vehicleForm = this.getNewFormGroup(this.vehicle, this.isDisabled);
        this.vehicleForm.updateValueAndValidity();
    }

    onSubmit() {
        this.checkLoading.setLoadingStatus(LoadingStatus.RequestOngoing);
        this.sendRequest[this.formAction!]();
    }

    private getNewFormGroup(vehicle: VehicleResponseDTO, isDisabled : boolean): FormGroup {
        return new FormGroup({
            id: new FormControl(vehicle.id),
            chassisSeries: new FormControl<string>({value: vehicle.chassisSeries, disabled: isDisabled}, [Validators.required, Validators.minLength(4), Validators.maxLength(30)]),
            chassisNumber: new FormControl<string>({value: vehicle.chassisNumber.toString(), disabled: isDisabled}, [Validators.required, Validators.minLength(3), Validators.maxLength(19)]),
            colorId: new FormControl<number>({value: vehicle.colorId, disabled: isDisabled}, [Validators.required, Validators.min(1)]),
            vehicleTypeId: new FormControl<number>({value: vehicle.vehicleTypeId, disabled: isDisabled}, [Validators.required, Validators.min(1)]),
        });
    }

    private async sendGetByIdRequest() {
        await this.getAuxiliaryData();
        this.checkLoading.setLoadingStatus(LoadingStatus.ResponseOngoing);
        const requestData: IRequestData<null> = {
            entity: 'vehicles',
            action: 'get',
            body: null,
            params: `/getbyid/${this.vehicle.id}`
        };

        this.dataService.run<null, VehicleResponseDTO>(requestData).subscribe({
            next: (response) => {
                this.vehicle = { ...response } as VehicleResponseDTO;
                this.vehicleForm = this.getNewFormGroup(this.vehicle, this.isDisabled);
            },
            error: (error) => {
                this.toastr.error('Error when trying to load the vehicle from database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
            }
        });
    }

    private sendCreateRequest() {
        var vehicleRequestDTO = new VehicleRequestDTO();
        vehicleRequestDTO = Object.assign({}, this.vehicleForm.value, {
            colorId: Number(this.vehicleForm.value.colorId),
            vehicleTypeId: Number(this.vehicleForm.value.vehicleTypeId),
        });

        const requestData: IRequestData<VehicleRequestDTO> = {
            entity: 'vehicles',
            action: 'post',
            body: vehicleRequestDTO,
            params: ``,
        };
        this.dataService.run<VehicleRequestDTO, null>(requestData).subscribe({
            next: () => {
                this.toastr.success('The vehicle name was saved.');
            },
            error: (error) => {
                this.toastr.error('Error when trying to save a new vehicle in the database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulRequest);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
                this.router.navigate(['/vehicles']);
            }
        });
    }

    private sendUpdateRequest() {
        var vehicleRequestDTO = new VehicleRequestDTO();
        Object.assign(vehicleRequestDTO, this.vehicleForm.value);
        const requestData: IRequestData<VehicleRequestDTO> = {
            entity: 'vehicles',
            action: 'put',
            body: vehicleRequestDTO,
            params: '',
        };
        this.dataService.run<VehicleRequestDTO, null>(requestData).subscribe({
            next: () => {
                this.toastr.success('The vehicle was updated.');
                this.router.navigate([this.backToListPath]);
            },
            error: (error) => {
                this.toastr.error('Error when trying to save the updated vehicle in the database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulRequest);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
            }
        });

    }

    private sendDeleteRequest() {
        const requestData: IRequestData<VehicleResponseDTO> = {
            entity: 'vehicles',
            action: 'delete',
            body: null,
            params: `?id=${this.vehicle.id}`,
        };
        this.dataService.run<VehicleResponseDTO, null>(requestData).subscribe({
            next: () => {
                this.toastr.success('The vehicle was deleted.');
                this.router.navigate([this.backToListPath]);
            },
            error: (error) => {
                this.toastr.error('Error when trying to delete the actual vehicle in the database: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulRequest);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
            }
        });

    }

    sendGetColors() {
        const requestData: IRequestData<null> = {
            entity: 'colors',
            action: 'get',
            body: null,
            params: ``
        };
        this.dataService.run<null, ColorResponseDTO[]>(requestData).subscribe({
            next: (response) => {
                this.colors = [...response] as ColorResponseDTO[];
            },
            error: (error) => {
                this.toastr.error('Error when trying to load data: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
            }
        });

    }

    sendGetVehicleTypes() {
        const requestData: IRequestData<null> = {
            entity: 'vehicleTypes',
            action: 'get',
            body: null,
            params: ``
        };
        this.dataService.run<null, VehicleTypeResponseDTO[]>(requestData).subscribe({
            next: (response) => {
                this.vehicleTypes = [...response] as VehicleTypeResponseDTO[];
            },
            error: (error) => {
                this.toastr.error('Error when trying to load data: ' + error.message);
                this.checkLoading.setLoadingStatus(LoadingStatus.UnsuccessfulResponse);
            },
            complete: () => {
                this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
            }
        });
    }

    // Gets data to be used on the select components
    async getAuxiliaryData() {
        await this.sendGetColors();
        await this.sendGetVehicleTypes();
    }

    updateLoadingStatus() {
        this.checkLoading.setLoadingStatus(LoadingStatus.Successful);
    }

    async onTryAgain() {
        await this.sendGetByIdRequest();
    }

    allowNumbersOnly(event: KeyboardEvent): void {
        const allowedKeys = [
            'Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Tab', 'Enter',
        ];
        const isNumber = /^\d$/.test(event.key);
        const isAllowedKey = allowedKeys.includes(event.key);

        if (!isNumber && !isAllowedKey) {
            event.preventDefault();
        }

        this.updateLoadingStatus();
    }

    allowPasteNumbersOnly(event: ClipboardEvent): void {
        const clipboardData = event.clipboardData?.getData('text') || '';
        if (!/^\d+$/.test(clipboardData)) {
            event.preventDefault();
        }
    }
}
