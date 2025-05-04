export enum RequestStatus {
  Approved = 0,
  Rejected = 1,
  Waiting = 2,
}

export interface BorrowingRequest {
  id: number;
  requestedDate: string;
  status: RequestStatus;
  requestorId: number;
  approverId: number;
  requestorUsername: string | null;
  approverUsername: string | null;
}

export interface BorrowingRequestToUpdate {
  status: RequestStatus;
}
