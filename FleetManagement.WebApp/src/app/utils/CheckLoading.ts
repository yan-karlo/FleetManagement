import { LoadingStatus } from "@app/Enums/loadingStatus.enum";

export class CheckLoading {
    private isLoading : LoadingStatus = LoadingStatus.ResponseOngoing;
    setLoadingStatus = (isLoading: LoadingStatus) : LoadingStatus => this.isLoading = isLoading;
    isUnsuccessful = (): boolean => this.isLoading === LoadingStatus.UnsuccessfulRequest || this.isLoading === LoadingStatus.UnsuccessfulResponse;
    isUnsuccessfulRequest = (): boolean => this.isLoading === LoadingStatus.UnsuccessfulRequest;
    isUnsuccessfulResponse = (): boolean => this.isLoading === LoadingStatus.UnsuccessfulResponse;
    isTryAgain = (): boolean => this.isLoading === LoadingStatus.TryAgain;
    isOngoing = (): boolean => this.isLoading === LoadingStatus.RequestOngoing || this.isLoading === LoadingStatus.ResponseOngoing;
    isRequestOngoing = (): boolean => this.isLoading === LoadingStatus.RequestOngoing;
    isResponseOngoing = (): boolean => this.isLoading === LoadingStatus.ResponseOngoing;
    isSuccessful = (): boolean => this.isLoading === LoadingStatus.Successful;
}