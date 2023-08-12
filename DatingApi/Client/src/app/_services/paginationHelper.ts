import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginationResult } from "../_models/pagination";

      

export function getPagination<T>(url: string, params: HttpParams, http: HttpClient) {
  const paginationResult: PaginationResult<T[]> = new PaginationResult<T[]>;
  return http.get<T[]>(url, { observe: 'response', params }).pipe(
    map((membersResponse) => {
      if (membersResponse.body) {
        paginationResult.result = membersResponse.body;
      }
      const pagination = membersResponse.headers.get('pagination');
      if (pagination) {
        paginationResult.pagination = JSON.parse(pagination);
      }
      return paginationResult;
    })
  );
}

export function getPaginationHeader(pageNumber: number, pageSize: number,) {
  let params = new HttpParams()
  params = params.append('pageNumber', pageNumber);
  params = params.append('pageSize', pageSize);
  return params;
}
