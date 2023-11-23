export type Question = {
    id: number;
    qAudio: string;
    qCategoryId: number;
    qDifficultyId: number;
    qTypeId: number;
    qImage: string;
    qStatement: string;
    qTime: number;
    details: QuestionDetail[];
};

export type QuestionDetail = {
    id: number;
    qDetailDesc: string;
    detailTypes: DetailType[];
};

export type DetailType = {
    id: number;
    detailDesc: string;
};

export type QuestionCategory = {
    id: number;
    qCategoryDesc: string;
};

export type QuestionDifficulty = {
    id: number;
    qDifficultyDesc: string;
};

export type QuestionType = {
    id: number;
    qTypeDesc: string;
};
export type QuestionCreateDto = {
    qAudio: string;
    qCategoryId: number;
    qDifficultyId: number;
    qTypeId: number;
    qImage: string;
    qStatement: string;
    qTime: number;
    questionDetailCreateDtos: QuestionDetailCreateDto[];
};
export type QuestionCreateValues = {
    qAudio: string;
    qCategoryId: string;
    qDifficultyId: string;
    qTypeId: string;
    qImage: string;
    qStatement: string;
    qTime: string;
    questionDetailCreateDtos: QuestionDetailCreateDto[];
    options: { value: string; isAnswer: boolean }[];
    trueOrFalseAnswer: boolean;
    minimum?: number;
    maximum?: number;
    interval?: number;
    sliderAnswer?: number;
    textToAudio?: string;
    language?: string;
    typeAnswer?: string;
};

export type QuestionDetailCreateDto = {
    qDetailDesc: string;
    detailTypes: string[];
};

export type QuestionResourceParameter = {
    pageSize: string;
    searchQuery?: string;
    pageNumber: number;
};

export type PaginationMetadata = {
    totalCount: number;
    pageSize: number;
    currentPage: number;
    totalPages: number;
};
