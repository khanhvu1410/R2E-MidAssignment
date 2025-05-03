import { ENDPOINT_API } from '../setup/config';
import { httpClient } from '../setup/httpClient';

export const getRequestDetailsByBorrowingRequestId = (
  borrowingRequestId: number
) => {
  return httpClient.get(
    ENDPOINT_API.requestDetails.getByBorrowingRequestId.replace(
      ':borrowingRequestId',
      borrowingRequestId.toString()
    )
  );
};
