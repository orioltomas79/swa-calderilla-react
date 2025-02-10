import {
  CurrentAccountsEndpointsClient,
  DevEndpointsClient,
  OperationsEndpointsClient,
} from "./apiClient.g.nswag";

class ApiClient {
  public readonly devEndpointsClient: DevEndpointsClient;
  public readonly operationsEndpointsClient: OperationsEndpointsClient;
  public readonly currentAccountEndpointsClient: CurrentAccountsEndpointsClient;

  constructor() {
    this.devEndpointsClient = new DevEndpointsClient("/api");
    this.operationsEndpointsClient = new OperationsEndpointsClient("/api");
    this.currentAccountEndpointsClient = new CurrentAccountsEndpointsClient("/api");
  }
}

const apiClient = new ApiClient();

export default apiClient;
