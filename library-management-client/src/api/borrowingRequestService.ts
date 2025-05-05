import { BorrowingRequestToUpdate } from '../models/borrowingRequest';
import { RequestDetailsToAdd } from '../models/requestDetails';
import { ENDPOINT_API } from '../setup/config';
import { httpClient } from '../setup/httpClient';

export const createBorrowingRequestService = async (
  requestDetails: RequestDetailsToAdd[]
) => {
  return await httpClient.post(
    ENDPOINT_API.borrowingRequest.create,
    requestDetails
  );
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

export const getBorrowingRequestsThisMonthService = async () => {
  return await httpClient.get(ENDPOINT_API.borrowingRequest.getThisMonth);
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

export const getBorrowingRequestByIdService = (id: number) => {
  return httpClient.get(
    ENDPOINT_API.borrowingRequest.getById.replace(':id', id.toString())
  );
};

export const updateBorrowingRequestService = (
  id: number,
  borrowingRequest: BorrowingRequestToUpdate
) => {
  return httpClient.patch(
    ENDPOINT_API.borrowingRequest.update.replace(':id', id.toString()),
    borrowingRequest
  );
};
