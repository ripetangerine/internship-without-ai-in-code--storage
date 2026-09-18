import { defineStore } from "pinia";
import { ref } from "vue";
import { useQuery, useMutation, useQueryClient } from "@tanstack/vue-query";

// TODO : 삭제

export const useProductStore = defineStore("product", () => {
  const queryClient = useQueryClient();

  // 1. Client State: 검색 및 필터 조건 (Pinia 관리)
  const searchQuery = ref("");
  const category = ref("all");

  // 2. Server State: Vue Query를 통한 API 데이터 관리
  // searchQuery나 category가 변경되면 Vue Query가 자동으로 API를 재호출함!
  const productsQuery = useQuery({
    queryKey: ["products", searchQuery, category], // 반응형 ref 전달
    queryFn: async () => {
      const res = await fetch(
        `/api/products?search=${searchQuery.value}&cat=${category.value}`,
      );
      if (!res.ok) throw new Error("데이터 불러오기 실패");
      return res.json();
    },
    staleTime: 1000 * 60 * 5, // 5분간 캐시 유지
  });

  // 3. Mutation: 데이터 수정 및 캐시 무효화
  const addProductMutation = useMutation({
    mutationFn: (newProduct) =>
      fetch("/api/products", {
        method: "POST",
        body: JSON.stringify(newProduct),
      }),
    onSuccess: () => {
      // 상품 추가 성공 시 'products' 관련 캐시를 즉시 무효화하여 자동 재요청
      queryClient.invalidateQueries({ queryKey: ["products"] });
    },
  });

  return {
    // State & Actions
    searchQuery,
    category,
    // Vue Query 객체 (isPending, isError, data 등 제공)
    productsQuery,
    addProduct: addProductMutation.mutate,
  };
});
