import {
  DevEndpointsClient,
  OperationsEndpointsClient,
} from "./apiClient.g.nswag";

class ApiClient {
  public readonly devEndpointsClient: DevEndpointsClient;
  public readonly operationsEndpointsClient: OperationsEndpointsClient;

  constructor() {
    this.devEndpointsClient = new DevEndpointsClient("/api");
    this.operationsEndpointsClient = new OperationsEndpointsClient("/api");
  }
}

const apiClient = new ApiClient();

export default apiClient;
