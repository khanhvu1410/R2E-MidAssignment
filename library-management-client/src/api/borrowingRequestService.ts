import { RequestDetailsToAdd } from '../models/requestDetails';
import { ENDPOINT_API } from '../setup/config';
import { httpClient } from '../setup/httpClient';

export const createBorrowingRequestService = (
  requestDetails: RequestDetailsToAdd[]
) => {
  return httpClient.post(ENDPOINT_API.borrowingRequest.create, requestDetails);
};

export const getPagedBorrowingRequestsService = (
  pageIndex: number,
  pageSize: number
) => {
  return httpClient.get(
    ENDPOINT_API.borrowingRequest.getPaged
      .replace(':pageIndex', pageIndex.toString())
      .replace(':pageSize', pageSize.toString())
  );
};

export const getBorrowingRequestsThisMonthService = () => {
  return httpClient.get(ENDPOINT_API.borrowingRequest.getThisMonth);
};

export const getBorrowingRequestsByRequestorId = (
  pageIndex: number,
  pageSize: number
) => {
  return httpClient.get(
    ENDPOINT_API.borrowingRequest.getByRequestorId
      .replace(':pageIndex', pageIndex.toString())
      .replace(':pageSize', pageSize.toString())
  );
};
