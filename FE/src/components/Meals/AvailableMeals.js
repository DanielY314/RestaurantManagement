import { useEffect, useState } from 'react';

import Card from '../UI/Card';
import MealItem from './MealItem/MealItem';
import classes from './AvailableMeals.module.css';

const AvailableMeals = () => {
  const [meals, setMeals] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [category, setCategory] = useState([]);
  const [, setIsLoadingCategory] = useState(true);
  const [httpError, setHttpError] = useState();
  const [selectedCategory, setSelectedCategory] = useState(0);

  let handleCategoryChange = (e) => {
    // console.log(e.target.value);
    setSelectedCategory(e.target.value);
  }

  useEffect(() => {
    const fetchCategory = async () => {
      const response = await fetch(
        `${process.env.REACT_APP_API_URL}/Category`
      );

      if (!response.ok) {
        throw new Error('Something went wrong!');
      }

      const responseData = await response.json();

      const loadedCategory = [];

      for (const key in responseData) {
        loadedCategory.push({
          id: responseData[key].categoryId,
          name: responseData[key].name,
          description: responseData[key].description,
        });
      }

      //console.log(loadedMeals);
      setCategory(loadedCategory);
      setIsLoadingCategory(false);
    };

    fetchCategory().catch((error) => {
      setIsLoadingCategory(false);
      setHttpError(error.message);
    });

    const fetchMeals = async () => {
      const targetUrl = selectedCategory === 0 ? `${process.env.REACT_APP_API_URL}/Food` : `${process.env.REACT_APP_API_URL}/Food/` + selectedCategory;
      console.log(targetUrl)
      const response = await fetch(
        //'https://react-http-e0a76-default-rtdb.firebaseio.com/meals.json'       
        targetUrl
      );
      
      if (!response.ok) {
        throw new Error('Something went wrong!');
      }

      const responseData = await response.json();

      const loadedMeals = [];

      for (const key in responseData) {
        loadedMeals.push({
          id: key,
          name: responseData[key].name,
          // description: responseData[key].description,
          price: responseData[key].sales_price,
        });
      }

      //console.log(loadedMeals);
      setMeals(loadedMeals);
      setIsLoading(false);
    };

    fetchMeals().catch((error) => {
      setIsLoading(false);
      setHttpError(error.message);
    });
  }, [selectedCategory]);

  if (isLoading) {
    return (
      <section className={classes.MealsLoading}>
        <p>Loading...</p>
      </section>
    );
  }

  if (httpError) {
    return (
      <section className={classes.MealsError}>
        <p>{httpError}</p>
      </section>
    );
  }

  const mealsList = meals.map((meal) => (
    <MealItem
      key={meal.id}
      id={meal.id}
      name={meal.name}
      // description={meal.description}
      price={meal.price}
    />
  ));

  return (
    <section className={classes.meals}>
      {/* {fruit} */}
      <br />
      <select onChange={handleCategoryChange}> 
        <option value="Select a category"> -- Select a category -- </option>
        {category.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
      </select>
      <Card>
        <ul>{mealsList}</ul>
      </Card>
    </section>
  );
};

export default AvailableMeals;
