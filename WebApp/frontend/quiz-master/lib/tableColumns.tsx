import { Question } from "./definitions";

export type TableColumn = {
  key: string;
  label: string;
  Render?: Function;
  className?: string;
};

export const questionTableColumns: TableColumn[] = [
  {
    key: "qStatement",
    label: "Question Statement",
    className: "w-[300px] lowercase first-letter:uppercase",
  },
  {
    key: "qTime",
    label: "Time Limit",
  },
  {
    key: "qTypeId",
    label: "Question Type",
    className: "lowercase first-letter:uppercase",
    Render: ({ value }: { value: Question }) => {
      return <div>{value.qTypeId}</div>;
    },
  },
  {
    key: "qCategoryId",
    label: "Category",
    Render: ({ value }: { value: Question }) => {
      return <div>{value.qCategoryId}</div>;
    },
  },
  {
    key: "qDifficultyId",
    label: "Difficulty",
    Render: ({ value }: { value: Question }) => {
      return <div>{value.qCategoryId}</div>;
    },
  },
];
