export class VehicleRequestDTO {
    id?: number = 0;
    chassisSeries: string = '';
    chassisNumber: string = '';
    vehicleTypeId: number = 0;
    colorId: number = 0

    constructor(data: Partial<VehicleRequestDTO> = {
        id : 0,
        chassisSeries : '',
        chassisNumber : '',
        vehicleTypeId : 0,
        colorId : 0,
     }) {
        Object.assign(this, data);
    }

}