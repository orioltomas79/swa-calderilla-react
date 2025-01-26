import { DevEndpointsClient } from "./apiClient.g.nswag";

class ApiClient {
  public readonly devEndpointsClient: DevEndpointsClient;

  constructor() {
    this.devEndpointsClient = new DevEndpointsClient("/api");
  }
}

const apiClient = new ApiClient();

export default apiClient;
