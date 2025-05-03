import { Category } from '../models/category';
import { ENDPOINT_API } from '../setup/config';
import { httpClient } from '../setup/httpClient';

export const getPagedCategoriesService = (
  pageIndex: number,
  pageSize: number
) => {
  return httpClient.get(
    ENDPOINT_API.category.getPaged
      .replace(':pageIndex', pageIndex.toString())
      .replace(':pageSize', pageSize.toString())
  );
};

export const getAllCategoriesService = () => {
  return httpClient.get(ENDPOINT_API.category.getAll);
};

export const createCategoryService = (category: Category) => {
  return httpClient.post(ENDPOINT_API.category.create, category);
};

export const deleteCategoryService = (id: number) => {
  return httpClient.delete(
    ENDPOINT_API.category.delete.replace(':id', id.toString())
  );
};

export const getCategoryByIdService = (id: number) => {
  return httpClient.get(
    ENDPOINT_API.category.getById.replace(':id', id.toString())
  );
};

export const updateCategoryService = (category: Category, id: number) => {
  return httpClient.put(
    ENDPOINT_API.category.update.replace(':id', id.toString()),
    category
  );
};
