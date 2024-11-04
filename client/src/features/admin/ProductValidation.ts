import * as yup from 'yup';

export const validationSchema = yup.object({
    name: yup.string().required(),
    brand: yup.string().required(),
    type: yup.string().required(),
    price: yup.number().required().moreThan(100).typeError('price must be greater than 100'),
    quantityInStock: yup.number().required().min(0).typeError('quantityInStock is a required field and must be between 0 - 200'),
    description: yup.string().required(),
    // file: yup.mixed().when('pictureUrl', {
    //     is: (value: string) => !value,
    //     then: yup.mixed().required('Please provide an image') as any
    //  })
   
});