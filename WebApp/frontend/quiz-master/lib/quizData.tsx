import {
  Question,
  QuestionCategory,
  QuestionDifficulty,
  QuestionType,
} from "./definitions";

export async function fetchQuestions() {
  console.log(process.env.QUIZMASTER_QUIZ);
  try {
    const data = await fetch(`${process.env.QUIZMASTER_QUIZ}/api/question`)
      .then((res) => res.json())
      .then((data) => {
        var questions: Question[];
        questions = data;
        return questions;
      });
    return data;
  } catch (error) {
    console.error("Database Error:", error);
    throw new Error("Failed to fetch question data.");
  }
}

export async function fetchCategories() {
  try {
    const data = await fetch(
      `${process.env.QUIZMASTER_QUIZ}/api/question/category`
    )
      .then((res) => res.json())
      .then((data) => {
        var categories: QuestionCategory[];
        categories = data;
        return categories;
      });
    return data;
  } catch (error) {
    console.error("Database Error:", error);
    throw new Error("Failed to fetch categories data.");
  }
}

export async function fetchDifficulties() {
  try {
    const data = await fetch(
      `${process.env.QUIZMASTER_QUIZ}/api/question/difficulty`
    )
      .then((res) => res.json())
      .then((data) => {
        var difficulties: QuestionDifficulty[];
        difficulties = data;
        return difficulties;
      });
    return data;
  } catch (error) {
    console.error("Database Error:", error);
    throw new Error("Failed to fetch difficulties data.");
  }
}

export async function fetchTypes() {
  try {
    const data = await fetch(`${process.env.QUIZMASTER_QUIZ}/api/question/type`)
      .then((res) => res.json())
      .then((data) => {
        var types: QuestionType[];
        types = data;
        return types;
      });
    return data;
  } catch (error) {
    console.error("Database Error:", error);
    throw new Error("Failed to fetch types data.");
  }
}
