"use client"
import React, { useState, useEffect } from 'react';
import { GetAllUsers } from '@/lib/hooks/dashboard-api-call'
import { fetchSets } from '@/lib/quizData';
import { useForm } from "@mantine/form";
import { fetchCategories } from '@/lib/quizData';
import { GetAllQuestion } from '@/lib/hooks/question';
import { fetchDifficulties } from '@/lib/hooks/difficulty';
import {
    CategoryResourceParameter
} from "@/lib/definitions";
import { json } from 'stream/consumers';
export default function Card() {
    const [usersLength, setUsersLength] = useState<number>(0);
    const [setsLength, setSetsLength] = useState<number>(0);
    const [questionsLength, setQuestionsLength] = useState<number>(0);
    const [categoriesLength, setCategoriesLength] = useState<number>(0);
    const [difficultiesLength,setDifficultiesLength] = useState<number>(0);

    const CategoriesForm = useForm<CategoryResourceParameter>({
        initialValues: {
            pageSize: "100",
            searchQuery: "",
            pageNumber: 1,
        },
    });

    const DifficultiesForm = useForm<CategoryResourceParameter>({
        initialValues: {
            pageSize: "100",
            searchQuery: "",
            pageNumber: 1,
        },
    });
    //Difficulties
    useEffect(() => {
        try {
            var difficulties = fetchDifficulties(DifficultiesForm.values);
            difficulties.then((res) => {
                setDifficultiesLength(res.data.length);
            });
        } catch (error) {}
    }, []);

    //categories
    useEffect(() => {
        var categoriesFetch = fetchCategories(CategoriesForm.values);
        categoriesFetch.then((res) => {
            setCategoriesLength(res.data.length);
        });
    }, []);

    // FOR USERS
    useEffect(() => {
        const fetchUsersLength = async () => {
          try {
            const users = await GetAllUsers();
            setUsersLength(users.length);

          } catch (error) {
            console.error(error);
          }
        };
    
     fetchUsersLength();
      }, []);
    //FOR SETS
      useEffect(() => {
        const fetchSetsLength = async () => {
        try {
            const set = await fetchSets();
            setSetsLength(set.length);
        } catch (error) {
            console.error(error);
        }
       
        }
        fetchSetsLength();
    }, []);
    //FOR QUESTIONS
    useEffect(() => {

      const fetchData = async () => {
        try {
          const response = await GetAllQuestion(); // Wait for the promise to resolve
          console.log("RESPONSE1121:", response );
          setQuestionsLength(response?.length);
          // Now you can work with the response data here
        } catch (error) {
          console.log(error);
        } 
      };
      fetchData(); // Call the async function
    }, []);
  
  return (
<div className="flex flex-wrap flex-col">
  <div className="mr-3 ml-3 mt-5 grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 justify-center">

    <div className="w-auto bg-white rounded overflow-hidden shadow-lg p-8 flex flex-col items-center border-l-2 border-[#FFAD3390] transition-transform hover:-translate-y-1">
      <div className="text-sm">All Users</div>
      <div className="text-2xl font-bold">{usersLength !== 0 || !usersLength ? usersLength : 0}</div>
    </div>
    <div className="w-auto bg-white rounded overflow-hidden shadow-lg p-8 flex flex-col items-center border-l-2 border-[#FFAD3390] transition-transform hover:-translate-y-1">
      <div className="text-md">Questions</div>
      <div className="text-2xl font-bold">{questionsLength !== 0 || !questionsLength ? questionsLength : 0}</div>
    </div>
    <div className="w-auto bg-white rounded overflow-hidden shadow-lg p-8 flex flex-col items-center border-l-2 border-[#FFAD3390] transition-transform hover:-translate-y-1">
      <div className="text-md">Categories</div>
      <div className="text-2xl font-bold">{categoriesLength !== 0 || !categoriesLength ? categoriesLength : 0}</div>
    </div>
    <div className="w-auto bg-white rounded overflow-hidden shadow-lg p-8 flex flex-col items-center border-l-2 border-[#FFAD3390] transition-transform hover:-translate-y-1">
      <div className="text-md">Question Sets</div>
      <div className="text-2xl font-bold">{setsLength !== 0 || !setsLength ? setsLength : 0}</div>
    </div>
    <div className="w-auto bg-white rounded overflow-hidden shadow-lg p-8 flex flex-col items-center border-l-2 border-[#FFAD3390] transition-transform hover:-translate-y-1">
      <div className="text-md">Difficulty Levels</div>
      <div className="text-2xl font-bold">{difficultiesLength !== 0 || !difficultiesLength ? difficultiesLength : 0}</div>
    </div>
  </div>
</div>


  
  )
}
