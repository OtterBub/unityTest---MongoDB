//
//  ImageTools.mm
//  NativeToolkit
//
//  Created by Ryan on 20/03/2014.
//
//

#import "ImageTools.h"
extern UIViewController *UnityGetGLViewController();

@implementation ImageTools

-(id)init {
    return [super init];
}

-(void)imagePickerController:(UIImagePickerController*)picker
didFinishPickingMediaWithInfo:(NSDictionary*)info{
    
    NSLog(@"**didFinishPickingImage**");
    
    UIImage *img = [self normalizeOrientation:[info objectForKey:UIImagePickerControllerOriginalImage]];
    NSString *path = [NSTemporaryDirectory() stringByAppendingPathComponent:@"image.jpg"];
    
    NSData *imageData = UIImageJPEGRepresentation(img, 0.7);
    [imageData writeToFile:path atomically:YES];
    
    const char *charPath = [path UTF8String];
    
    UnitySendMessage("NativeToolkit", "OnPickImage", charPath);
    
    [picker dismissModalViewControllerAnimated:YES];
}

-(void)imagePickerControllerDidCancel:(UIImagePickerController*)picker
{
    NSString *path = @"Cancelled";
    const char *charPath = [path UTF8String];
    UnitySendMessage("NativeToolkit", "OnPickImage", charPath);
    
    [picker dismissModalViewControllerAnimated:YES];
}

-(UIImage *)normalizeOrientation:(UIImage *)img {
    if (img.imageOrientation == UIImageOrientationUp)
        return img;
    
    UIGraphicsBeginImageContextWithOptions(img.size, NO, img.scale);
    [img drawInRect:(CGRect){0, 0, img.size}];
    UIImage *normalizedImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    return normalizedImage;
}

@end

static ImageTools *imageToolsDelegate = NULL;
UIPopoverController *popover;

extern "C"
{
    bool saveToGallery(char* path)
    {
        NSString *imagePath = [NSString stringWithUTF8String:path];
        
        //NSLog(@"##This is the file path being passed: %@", imagePath);
        
        if( ![[NSFileManager defaultManager] fileExistsAtPath:imagePath] ) {
            return false;
        }
        
        UIImage *image = [UIImage imageWithContentsOfFile:imagePath];
        
        if( image ) {
            UIImageWriteToSavedPhotosAlbum( image, nil, NULL, NULL );
            NSLog(@"**Image saved**");
            return true;
        }
        
        return false;
    }
    
    void pickImage()
    {
        NSLog(@"**pickImage**");
        
        if(imageToolsDelegate == NULL) imageToolsDelegate = [[ImageTools alloc] init];
        
        UIImagePickerController *imagePicker = [[UIImagePickerController alloc] init];
        
        if ([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeSavedPhotosAlbum])
            imagePicker.sourceType = UIImagePickerControllerSourceTypeSavedPhotosAlbum;
        else
            imagePicker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
        
        imagePicker.delegate = imageToolsDelegate;
        
        if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPad)
        {
            popover = [[UIPopoverController alloc] initWithContentViewController:imagePicker];
            [popover presentPopoverFromRect:CGRectZero inView:UnityGetGLViewController().view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
        }
        else
        {
            [UnityGetGLViewController() presentModalViewController:imagePicker animated:YES];
        }
    }
}