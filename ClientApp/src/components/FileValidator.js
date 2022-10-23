import {useState} from 'react';
import axios from 'axios';
import { toast} from 'react-toastify';

import './style.css';

  export const FileValidator = ({onSuccess}) => {
    
    const [files, setFiles] = useState([]);
    
    // Define varibles to hold folder contents
    const [dll, setDll] = useState([]);
    const [images, setImg] = useState([]);
    const [lang, setLang] = useState([]);

    // Define variables to hold validation errors
    const [dllErrors, setDllErrors] = useState([]);
    const [imagesErrors, setImagesErrors] = useState([]);
    const [langErros, setLangErrors] = useState([]);
    const [errors, setErrors] = useState([]);
    const [structureErrors, setstructureErrors] = useState([]);

    const [disable, setDisable] = useState(0)

    const [isValidationFailed, setIsValidationFailed] = useState([]);
    const [isNotShowStructure, setIsNotShowStructure] = useState([]);

    // Event triggered when user choose a file using the file uploader
    const onInputChange = (e) => {
        setIsValidationFailed(true)
        setFiles(e.target.files)
        setDisable(e.target.files.length)
        console.log(e.target.files.length);
        setIsNotShowStructure(true)
    };

    // Event triggered when user clicks the save button
    const onClickSave = (e) => {
        e.preventDefault();

        const data = new FormData();

        for(let i = 0; i < files.length; i++) {
            data.append('file', files[i]);
        }
       
        axios.post('//localhost:44466/file/save', data)
            .then((response) => {

                toast.success('Save Success');
                onSuccess(response.data)
            })
            .catch((e) => {
                toast.error('Save Error')
            })
    };

    // Event triggered when user clicks the validate button
    const onSubmit = (e) => {
        e.preventDefault();
        setIsNotShowStructure(false)
        const data = new FormData();

        for(let i = 0; i < files.length; i++) {
            data.append('file', files[i]);
        }

        // Send REST call to the backend to validate the uploaded file
        axios.post('//localhost:44466/file/', data)
            .then((response) => {
                
                // Populate variables retrieved from the response
                setDll(response.data.dllsContent)
                setImg(response.data.imagesContent)
                setLang(response.data.languagesContent)
                
                setDllErrors(response.data.dllsErrors)
                setImagesErrors(response.data.imagesErrors)
                setLangErrors(response.data.languagesErrors)
                setErrors(response.data.errors)
                setstructureErrors(response.data.structureErrors)
                setIsValidationFailed(response.data.isValidationFailed)

                toast.success('Upload Success');
                onSuccess(response.data)
            })
            .catch((e) => {
                toast.error('Upload Error')
            })
    };

    return (
        
        // Render the UI
        <div>
            <h1>File Validator</h1>
            <p>Welcome to your smart file validator</p>
            <p>To get started, please follow the instruction given below,</p>
            <ul>
            <li><strong>Upload</strong>. Upload your zip file to the app</li>
            <li><strong>Validate</strong>. Validate the file</li>
            <li><strong>Save</strong>. Save the file</li>
            </ul>
            <hr/>
            <div className="formdiv">
            <form className="form" method="post" action="#" id="#" onSubmit={onSubmit}>
                <div className="form-group files">
                    <label>Upload Your File </label>
                    <input type="file"
                        onChange={onInputChange}
                        className="form-control"
                        multiple/>
                </div>
                {isValidationFailed && <button type="submit" disabled={disable === 0}>Validate</button>}
                {!isValidationFailed && <button onClick={onClickSave}>Save</button>}
            </form>

            {!isValidationFailed && <label class="validatedLabel"> &nbsp;Successfully Validated&nbsp;</label>}
            <br/>
            {errors.length>0 && !isNotShowStructure &&  <p>Errors (Fix errors and try again)</p>}
            {errors.map(item => {
                                return !isNotShowStructure && <p style={{color:"red"}}><strong>{item}</strong></p>;
                            })}
            {structureErrors.length>0 && !isNotShowStructure && <p>Strucuture errors <br/> (Remove extra files or folders mentioned below. If any folder is missing, add it and try again)</p>}                
            {structureErrors.map(item => {
                                return !isNotShowStructure && <p style={{color:"red"}}><small>{item}</small></p>;
                            })}
            </div>

            <div class="vl"></div>
            {!isNotShowStructure && !(errors.length > 0) && <div className="fileStr">
            <br/>
                <div>
                    <p><strong>Folder Strucuture</strong></p>
                
                        <p>dlls</p>
                        <ul>
                            {dll.map(item => {
                                return <li>{item}</li>;
                            })}
                        </ul>
                        
                        {dllErrors.map(item => {
                                return <p style={{color:"red"}}><small>{item}</small></p>;
                            })}
                        <p>Images</p>
                        <ul>
                            {images.map(item => {
                                return <li>{item}</li>;
                            })}
                        </ul>
                        {imagesErrors.map(item => {
                                return <p style={{color:"red"}}><small>{item}</small></p>;
                            })}
                        <p>Languages</p>
                        <ul>
                            {lang.map(item => {
                                return <li>{item}</li>;
                            })}
                        </ul>
                        {langErros.map(item => {
                                return <p style={{color:"red"}}><small>{item}</small></p>;
                            })}
                    </div>

            </div>}
        </div>
    )
};