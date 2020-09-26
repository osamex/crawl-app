export interface IResponseInterface {
    Id: string;
    Created: Date;
    RequestId: string;
    RequestCreated: Date;
    IsSuccess: boolean;
    ResponseData: any;
    Errors: string[];
}
