import { Question } from "@/lib/definitions";
import { fetchQuestions } from "@/lib/quizData";
import { questionTableColumns } from "@/lib/tableColumns";
import { useEffect, useState } from "react";

export default function QuestionTable() {
  const [questions, setQuestions] = useState<Question[]>([]);
  useEffect(() => {
    var questionsFetch = fetchQuestions();
    questionsFetch.then((res) => {
      console.log(res);
      setQuestions(res);
    });
  }, []);

  return (
    <div className="w-full border-2 rounded-xl overflow-x-auto">
      <table className="w-full ">
        <thead>
          <tr className="table-row font-bold text-black border-b border">
            {questionTableColumns.map((column, index) => (
              <th
                key={index}
                className={`table-cell py-[10px] ${
                  column.className ? column.className : ""
                }`}
              >
                {column.label}
              </th>
            ))}
            {/* <th>
              <div
                className={`${
                  isEditMode ? "w-[150px]" : "w-0 "
                }  transition-all duration-1000 overflow-hidden`}
              >
                Actions
              </div>
            </th> */}
          </tr>
        </thead>
        <tbody>
          {questions.map((question, index) => (
            <>
              <tr key={index} className="table-row bg-white ">
                {questionTableColumns.map((column, index2) => (
                  <td
                    key={index2}
                    className={`px-2 py-2 table-cell ${
                      column.className && column.className
                    }`}
                  >
                    {column.Render
                      ? column.Render({ value: question })
                      : (question as any)[column.key]}
                  </td>
                ))}
                <td>
                  {/* <div
                    className={`${
                      isEditMode ? "w-[150px]" : "w-0 "
                    }  transition-all duration-1000 overflow-hidden`}
                  >
                    <div className="flex w-min">
                      <IconButton
                        variant="text"
                        color="green"
                        disabled={isLoading}
                        onClick={() => handleLike(contact)}
                      >
                        {contact.isFavorite && (
                          <FavoriteRoundedIcon
                            fontSize="medium"
                            color="success"
                          />
                        )}
                        {!contact.isFavorite && (
                          <FavoriteBorderRoundedIcon
                            fontSize="medium"
                            color="success"
                          />
                        )}
                      </IconButton>
                      <Link href={`/contacts/edit-contact/${contact.id}`}>
                        <IconButton variant="text" color="green">
                          <ModeEditOutlineRoundedIcon fontSize="medium" />
                        </IconButton>
                      </Link>
                      <IconButton
                        variant="text"
                        color="red"
                        onClick={() => handleDelete(contact)}
                      >
                        <DeleteOutlineRoundedIcon fontSize="medium" />
                      </IconButton>
                    </div>
                  </div> */}
                </td>
              </tr>
            </>
          ))}
        </tbody>
      </table>
    </div>
  );
}
