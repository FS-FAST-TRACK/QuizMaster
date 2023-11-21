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

export type QuestionCreateValues = {
    qAudio: string;
    qCategoryId: string;
    qDifficultyId: string;
    qTypeId: string;
    qImage: string;
    qStatement: string;
    qTime: string;
    questionDetailCreateDtos: QuestionDetailCreateDto[];
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
    TotalCount: number;
    PageSize: number;
    CurrentPage: number;
    TotalPages: number;
};
