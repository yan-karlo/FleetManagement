import * as appEndpoints from '@configs/app.endpoints.config.json';
export class AppEndpointResolver {
    static resolve(endpoint: string, params: Record<string, any> = {}): string {
      return endpoint.replace(/:([a-zA-Z]+)/g, (_, key) => {
        if (params[key] === undefined) {
          throw new Error(`Missing parameter for placeholder ":${key}"`);
        }
        return params[key];
      });
    }
}