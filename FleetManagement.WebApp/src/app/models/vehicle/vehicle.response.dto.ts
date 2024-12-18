export class VehicleResponseDTO {
    id: number = 0;
    chassis: string = '';
    chassisSeries: string = '';
    chassisNumber: string = '';
    vehicleTypeId: number = 0;
    vehicleTypeName: string = ''
    vehicleTypePassengersCapacity: number = 0
    colorId: number = 0
    colorName: string = '';

    constructor(data: Partial<VehicleResponseDTO> = {
        id : 0,
        chassis : '',
        chassisSeries : '',
        chassisNumber : '',
        vehicleTypeId : 0,
        vehicleTypeName : '',
        vehicleTypePassengersCapacity : 0,
        colorId : 0,
        colorName : ''
     }) {
        Object.assign(this, data);
    }

}