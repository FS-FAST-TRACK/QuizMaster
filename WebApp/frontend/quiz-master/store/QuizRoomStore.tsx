import { QuestionDifficulty } from "@/lib/definitions";
import { QuizRoom } from "@/lib/definitions/quizRoom";
import { create } from "zustand";

interface IQuizRoomsStore {
    quizRooms?: QuizRoom[];
    getPaginatedRooms: ({
        pageNumber,
        pageSize,
        searchQuery,
    }: {
        pageNumber: number;
        pageSize: number;
        searchQuery?: string;
    }) => QuizRoom[];
    setQuizRooms: (fetchedQuizRooms: QuizRoom[]) => void;
    pageNumber: number;
    pageSize: number;
    searchQuery?: string;
    getTotalPages: () => number;
    setPagination: ({
        pageNumber,
        pageSize,
        searchQuery,
    }: {
        pageNumber: number;
        pageSize: number;
        searchQuery?: string;
    }) => void;
}

export const useQuizRoomsStore = create<IQuizRoomsStore>((set, get) => ({
    quizRooms: undefined,
    setQuizRooms: (fetchedQuizRooms: QuizRoom[]) => {
        set({
            quizRooms: fetchedQuizRooms,
        });
    },
    getPaginatedRooms: ({
        pageNumber,
        pageSize,
        searchQuery,
    }: {
        pageNumber: number;
        pageSize: number;
        searchQuery?: string;
    }) => {
        return get().quizRooms
            ? get()
                  .quizRooms!.filter((qR) =>
                      searchQuery
                          ? qR.qRoomDesc
                                .trim()
                                .toLowerCase()
                                .includes(searchQuery)
                          : true
                  )
                  .slice(
                      (pageNumber - 1) * pageSize,
                      (pageNumber - 1) * pageSize + pageSize
                  )
            : [];
    },
    pageNumber: 1,
    pageSize: 10,
    searchQuery: "",
    getTotalPages: () =>
        get().quizRooms !== undefined
            ? Math.ceil(
                  get().quizRooms!.filter((qR) =>
                      get().searchQuery
                          ? qR.qRoomDesc
                                .trim()
                                .toLowerCase()
                                .includes(get().searchQuery!)
                          : true
                  ).length / get().pageSize
              )
            : 0,
    setPagination: ({
        pageNumber,
        pageSize,
        searchQuery,
    }: {
        pageNumber: number;
        pageSize: number;
        searchQuery?: string;
    }) => {
        if (
            get().quizRooms === undefined ||
            pageNumber > Math.ceil(get().quizRooms!.length / pageSize)
        ) {
            return;
        }

        set({
            pageNumber:
                searchQuery !== undefined || searchQuery !== ""
                    ? 1
                    : pageNumber,
            pageSize: pageSize,
            searchQuery: searchQuery,
        });
    },
}));
